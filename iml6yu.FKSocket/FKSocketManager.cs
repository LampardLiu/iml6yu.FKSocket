using iml6yu.FKSocket.Clients;
using iml6yu.FKSocket.Servers;
using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.FKSocket
{
    public class FKSocketManager
    {
        /// <summary>
        /// 创建一个socket客户端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IFKSocketClient CreateClient(SocketOption option)
        {
            return new FKSocket.Clients.SocketClient.FKSocketClient(option);
        }

        /// <summary>
        /// 创建一个socket服务端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IFKSocketServer CreateServer(SocketOption option)
        {
            throw new NotImplementedException();
        }
    }
}
