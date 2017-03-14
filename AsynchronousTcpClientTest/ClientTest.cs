using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using NUnit.Framework;
using AsynchronousTcpClient;
namespace AsynchronousTcpClientTest
{
    [TestFixture]
    public class ClientTest
    {
        [Test]
        public void ConnectToServer()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry("www.google.com");
            IPAddress ip = ipEntry.AddressList[0];
            AsynchronousTcpClient.Client tcpClient = new Client(ip, 11000);
            tcpClient.Connect();
        }
    }
}
