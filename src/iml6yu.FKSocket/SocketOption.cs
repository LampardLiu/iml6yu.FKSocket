using iml6yu.FKSocket.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace iml6yu.FKSocket
{
    public class SocketOption : ISocktOption
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 连接类型
        /// </summary>
        public ProtocolType ConnectType { get; set; } = ProtocolType.Tcp;

        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 是否自动重连
        /// </summary>
        public bool AutoReConnection { get; set; }

        /// <summary>
        /// 最大自动重连次数
        /// <para>
        /// 默认5次
        /// </para>
        /// </summary>
        public int ReConnectionMax { get; set; } = 5;
    }

    public class SocketHeartOption : SocketOption, ISocktHeartOption
    {
        /// <summary>
        /// 心跳检查间隔
        /// 默认10s
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(10);
    }
}
