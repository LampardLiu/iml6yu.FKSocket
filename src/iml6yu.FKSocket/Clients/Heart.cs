using iml6yu.FKSocket.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iml6yu.FKSocket.Clients
{
    /// <summary>
    /// 心跳管理
    /// </summary>
    public class HeartClientManager : IDisposable
    {
        /// <summary>
        /// 循环标志
        /// </summary>
        private System.Threading.CancellationTokenSource _token;

        /// <summary>
        /// 配置
        /// </summary>
        private ISocktHeartOption _option;

        private bool _currentHeartState;
        /// <summary>
        /// 心跳状态改变
        /// </summary>
        public event Action<bool, string> Changed;


        public HeartClientManager(ISocktHeartOption option)
        {
            _option = option;
        }

        public void Dispose()
        {
            _token?.Cancel();
            _token?.Dispose();
        }

        public virtual void UseHeart(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));
            if (!socket.Connected)
            {
                Changed?.Invoke(false, $"{nameof(socket)}未连接");
                return;
            }
            socket.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveData(), null);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Task.Run(() =>
            {
                _token = new System.Threading.CancellationTokenSource();
                while (_token != null && !_token.IsCancellationRequested)
                {
                    var state = IsConnect(socket);
                    if (_currentHeartState != state)
                        Changed?.Invoke(state, state ? "心跳正常" : "心跳断开");
                    _currentHeartState = state;
                    Task.Delay(_option.Interval).Wait();
                }
            });

        }

        public void StopHeart()
        {
            _token?.Cancel();
            _token?.Dispose();
            _token = null;
        }

        private byte[] GetKeepAliveData()
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)_option.Interval.TotalMilliseconds).CopyTo(inOptionValues, Marshal.SizeOf(dummy));//keep-alive间隔
            BitConverter.GetBytes((uint)500).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);// 尝试间隔
            return inOptionValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private bool IsConnect(Socket socket)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation c in tcpConnections)
            {
                TcpState stateOfConnection = c.State;
                if (c.LocalEndPoint.Equals(socket.LocalEndPoint) && c.RemoteEndPoint.Equals(socket.RemoteEndPoint))
                    return stateOfConnection == TcpState.Established;
            }
            return false;
        }
    }
}
