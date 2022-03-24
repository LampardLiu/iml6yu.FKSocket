using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace iml6yu.FKSocket.Interfaces
{
    public interface ISocktOption
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public ProtocolType ConnectType { get; set; }

        public TimeSpan Timeout { get; set; }

        public Encoding Encoding { get; set; }
    }

    public interface ISocktHeartOption : ISocktOption
    {
        public TimeSpan Interval { get; set; }
    }
}
