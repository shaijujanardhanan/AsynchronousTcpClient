using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            Debug.WriteLine($"Client connected to {client.RemoteEndPoint}");
            connectDone.Set();
        }

        public void SendData(string data)
        {
            byte[] dataByte = Encoding.ASCII.GetBytes(data);
            socket.BeginSend(dataByte, 0, dataByte.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int bytesSent = client.EndReceive(ar);
            Debug.WriteLine($"Client sent {bytesSent} to server");
            sendDone.Set();
        }

        public string ReceiveData()
        {
            byte[] receivedData = new byte[1024];
            socket.BeginReceive(receivedData, 0, receivedData.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            receiveDone.WaitOne();
            return BitConverter.ToString(receivedData);

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket client = (Socket) ar.AsyncState;
            int bytesReceived = client.EndReceive(ar);
            receiveDone.Set();
        }
    }
}
