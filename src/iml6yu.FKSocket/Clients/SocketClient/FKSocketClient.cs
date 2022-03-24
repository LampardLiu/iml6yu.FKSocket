using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iml6yu.FKSocket.Clients.SocketClient
{
    public class FKSocketClient : IFKSocketClient
    {
        private Socket _client;
        private SocketOption _socketOption;
        private CancellationTokenSource _token;
        private HeartClientManager _heartManager;

        private bool _isConnected;
        private int _reCount;

        public bool IsConnected => _isConnected;

        public event Action<bool, string> ConnectStateChanged;
        public event Action<string> Received;
        public event Action<Exception> ReceiveException;
        public event Action<int> ReConnectioned;

        internal FKSocketClient(SocketOption option)
        {
            _socketOption = option;
            this.ConnectStateChanged += FKSocketClient_ConnectStateChanged;
        }

        private void FKSocketClient_ConnectStateChanged(bool flag, string msg)
        {
            if (flag)
                _reCount = 0;
        }

        /// <summary>
        /// 进行连接
        /// </summary>
        /// <returns></returns>
        public IFKSocketClient Connect()
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, _socketOption.ConnectType);
                _client.Connect(_socketOption.Host, _socketOption.Port);
                _token = new CancellationTokenSource();
                StartReceive();
            }
            catch (Exception ex)
            {
                _isConnected = false;
                ConnectStateChanged?.Invoke(_isConnected, ex.Message);
            }
            return this;
            //开始接受数据
            void StartReceive()
            {
                Task.Run(() =>
                {
                    while (IsConnected && !_token.IsCancellationRequested)
                    {
                        byte[] buffer = new byte[2048];
                        var length = _client.Receive(buffer);
                        if (length == 0) continue;
                        var message = Encoding.UTF8.GetString(buffer);
                        Task.Run(() => { Received?.Invoke(message); });
                    }
                }).ContinueWith(t =>
                {
                    if (_socketOption.AutoReConnection && _socketOption.ReConnectionMax < _reCount++)
                    {
                        Connect().OpenHeartCheck();
                        ReceiveException?.Invoke(t.Exception);
                    }


                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        public void DisConnection()
        {
            _reCount = _socketOption.ReConnectionMax;
            if (_client != null && IsConnected)
            {
                _client.Shutdown(SocketShutdown.Both);
                _heartManager?.StopHeart();
            }
        }

        /// <summary>
        /// 打开心跳检查
        /// </summary>
        /// <param name="interval">
        /// 检测间隔，默认保持10s
        /// </param>
        /// <returns></returns>
        public IFKSocketClient OpenHeartCheck(uint interval = 10000)
        {
            if (_heartManager != null)
                _heartManager.Dispose();
            _heartManager = new HeartClientManager(new SocketHeartOption()
            {
                ConnectType = _socketOption.ConnectType,
                Host = _socketOption.Host,
                Port = _socketOption.Port,
                Encoding = _socketOption.Encoding,
                Interval = TimeSpan.FromMilliseconds(interval),
                Timeout = _socketOption.Timeout,
            });
            _heartManager.Changed += (flag, msg) =>
            {
                this.ConnectStateChanged?.Invoke(flag, msg);
                _isConnected = flag;
                if (!_isConnected && _socketOption.AutoReConnection && _socketOption.ReConnectionMax > _reCount++)
                {
                    ReConnectioned?.Invoke(_reCount);
                    Connect().OpenHeartCheck();
                }
            };
            _heartManager.UseHeart(_client);
            return this;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Send(string message)
        {
            if (_client == null)
                throw new Exception("客户端未初始化");
            if (!_client.Connected)
            {
                ConnectStateChanged?.Invoke(false, "连接已经被断开");
                return false;
            }
            return _client.Send(_socketOption.Encoding.GetBytes(message)) > 0;
        }

        public void OpenAutoReConnection(int reConnectionMax)
        {
            _socketOption.AutoReConnection = true;
            _socketOption.ReConnectionMax = reConnectionMax;
        }

        public void Dispose()
        {
            if (this._client != null && _client.Connected)
                _client.Shutdown(SocketShutdown.Both);
            _token?.Cancel();
            _token?.Dispose();
        }
    }
}
