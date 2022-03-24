using iml6yu.FKSocket.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace iml6yu.FKSocket
{
    public class SocketOption : ISocktOption
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public ProtocolType ConnectType { get; set; } = ProtocolType.Tcp;

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }

    public class SocketHeartOption : SocketOption, ISocktHeartOption
    {
        public TimeSpan Interval { get; set; }
    } 
}
