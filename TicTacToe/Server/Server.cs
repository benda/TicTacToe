using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Server
    {
        public void Run()
        {
            TcpListener clientListener = null;
            TcpClient client = null;
            ManualResetEvent endOfTurnWait = new ManualResetEvent(false);
            try
            {
                clientListener = new TcpListener(Dns.GetHostAddresses(Dns.GetHostName())[0], 311);
                clientListener.Start();


                while (true)
                {
                    if (clientListener.Pending())
                    {
                        client = clientListener.AcceptTcpClient();
                        string thisClientIP = client.Client.RemoteEndPoint.ToString();
                        //                    NetworkCommunicator networkCommunicator = new NetworkCommunicator(client);


                        Thread playerThread = new Thread(ClientGameLoop);
                        playerThread.IsBackground = true;
                        playerThread.Start(client);
                    }
                }
            }
            finally
            {
                clientListener.Stop();
            }
        }

        private void ClientGameLoop(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
        }
    }
}
