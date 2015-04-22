using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace EnCorTest
{
    public static class Utils
    {
        public static int GetFreeTcpPort()
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
            Random random = new Random();
            int loopTimeout = 20;
            bool found = false;
            int port = 0;
            while (!found && loopTimeout > 0)
            {
                port = random.Next(30000, 65500);
                found = !ipGlobalProperties.GetActiveTcpConnections().Any<TcpConnectionInformation>( x => x.LocalEndPoint.Port == port);

                if (found)
                {
                    break;
                }
            }
            return port;
        }
    }
}
