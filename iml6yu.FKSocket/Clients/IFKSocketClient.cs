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
        /// 接收到的数据
        /// </summary>
        event Action<string> Received;

        /// <summary>
        /// 进行连接
        /// </summary>
        /// <returns></returns>
        IFKSocketClient Connect();


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

    }
}
