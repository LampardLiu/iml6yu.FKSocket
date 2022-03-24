using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.FKSocket.Clients
{
    public interface IFKSocketClient : IDisposable
    {
        bool IsConnected { get; }

        /// <summary>
        /// 连接状态发生变化
        /// <para>
        /// true:成功
        /// </para>
        ///<para>
        ///false:失败
        /// </para>
        /// </summary>
        event Action<bool, string> ConnectStateChanged;

        /// <summary>
        /// 发生重连时通知
        /// 参数：int 重连次数
        /// </summary>
        event Action<int> ReConnectioned;

        /// <summary>
        /// 接收到的数据
        /// </summary>
        event Action<string> Received;

        /// <summary>
        /// 进行连接
        /// </summary>
        /// <returns></returns>
        IFKSocketClient Connect();

        /// <summary>
        /// 断开连接
        /// </summary>
        void DisConnection();

        /// <summary>
        /// 打开心跳检查
        /// </summary>
        /// <param name="interval">
        /// 检测间隔，默认保持10s
        /// </param>
        /// <returns></returns>
        IFKSocketClient OpenHeartCheck(uint interval = 10000);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        bool Send(string message);

        /// <summary>
        /// 打开自动重连机制
        /// <paramref name="repeatMax">最大重试次数</paramref>
        /// </summary>
        void OpenAutoReConnection(int repeatMax);

    }
}
