using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace iml6yu.FKSocket.Interfaces
{
    /// <summary>
    /// 配置
    /// </summary>
    public interface ISocktOption
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
        public ProtocolType ConnectType { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 是否自动重连
        /// </summary>
        public bool AutoReConnection { get; set; }

        /// <summary>
        /// 最大自动重连次数
        /// </summary>
        public int ReConnectionMax { get; set; }
    }

    public interface ISocktHeartOption : ISocktOption
    {
        public TimeSpan Interval { get; set; }
    }
}
