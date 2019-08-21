using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;


namespace TicTacToe.Net.NetScanning
{
    public class NetScan
    {
        public event EventHandler<NetScanPingCompletedEventArgs> PingComplete;
        public event EventHandler<EventArgs> NetScanComplete;
       
        const int MaxSimultaneousPings = 200;
        private Semaphore pingBlock = new Semaphore(0, MaxSimultaneousPings);

        public NetScan()
        {
            pingBlock.Release(MaxSimultaneousPings);
        }

        public void Start(PingRange pr)
        {
             Thread t = new Thread(new ParameterizedThreadStart(pingWorker_DoWork));
             t.Start(pr);
        }

        private void pingWorker_DoWork(object o)
        {
            PingRange pingRange = (PingRange)o;

            byte[] start = pingRange.StartRange.GetAddressBytes();
            byte[] end = pingRange.EndRange.GetAddressBytes();

            LinkedList<Thread> threads = new LinkedList<Thread>();

            for (byte o0 = start[0]; o0 <= end[0]; o0++)
                for (byte o1 = start[1]; o1 <= end[1]; o1++)
                    for (byte o2 = start[2]; o2 <= end[2]; o2++)
                        for (byte o3 = start[3]; o3 <= end[3]; o3++)
                        {
                            pingBlock.WaitOne();
                            Thread t = new Thread(new ParameterizedThreadStart(DoPing));
                            t.Start(new IPAddress(new byte[] { o0, o1, o2, o3 }));
                            threads.AddLast(t);
                        }


            foreach (Thread ln in threads)
            {
                ln.Join();
            }

            try
            {
                RaiseNetScanComplete();      
            }
            catch (InvalidOperationException)
            {

            }

        }


        private void DoPing(object o)
        {
            IPAddress ip = (IPAddress)o;
            Ping ping = new Ping();
            PingReply pr = ping.Send(ip);
            pingBlock.Release();
            try
            {
                bool serverIsHostingGame = false;
                if (pr.Status == IPStatus.Success)
                {/*
                    if (ServerConnector.ServerIsHostingGame(ip))
                    {
                        serverIsHostingGame = true;
                    }*/
                }
               RaisePingComplete(pr, serverIsHostingGame);
                    
            }
            catch (InvalidOperationException)
            {
             
            }
        }


        private delegate void RaiseNetScanCompleteHandler();
        private void RaiseNetScanComplete()
        {
            NetScanComplete(this, new EventArgs());
        }



        private delegate void RaisePingCompleteHandler(PingReply pr, bool serverIsHostingGame);
        private void RaisePingComplete(PingReply pr, bool serverIsHostingGame)
        {
            NetScanPingCompletedEventArgs e = new NetScanPingCompletedEventArgs();
            e.Reply = pr;
            e.ServerIsHostingGame = serverIsHostingGame;
            PingComplete(this, e);
        }

      }
}
