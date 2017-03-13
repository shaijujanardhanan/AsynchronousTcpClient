using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousTcpClient
{
    public class Client
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        Int32 port = 0;
        Socket socket = null;
        static ManualResetEvent connectDone = new ManualResetEvent(false);
        static ManualResetEvent sendDone = new ManualResetEvent(false);
        static ManualResetEvent receiveDone = new ManualResetEvent(false);
        public Client(IPAddress ipAddress, Int32 portNo)
        {
            this.ip = ipAddress;
            this.port = portNo;
        }

        public void Connect()
        {
            IPEndPoint remoteEP = new IPEndPoint(ip, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), socket);
            connectDone.WaitOne();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Socket client = ar as Socket;
            client.EndConnect(ar);
            connectDone.Set();
        }
    }
}
