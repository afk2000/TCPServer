using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCPServer.Library;

namespace TCPServer
{
    class Program
    {
        private const string IP_ADDRESS = "127.0.0.1";
        private const int PORT_NUMBER = 4000;
        ConcurrentQueue<Connection> queue = new ConcurrentQueue<Connection>(); // FIFO collection 

        //ManualResetEvent reset = new ManualResetEvent(true);
        private int MaxConnectionsAllowed;
        private bool ServerRunning = true;
        static void Main(string[] args)
        {

            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine(options.GetUsage());
                return;
            }

            new Program().Start(options);
        }


        void Start(CommandLineOptions options)
        {
            Console.WriteLine("Server Start");
            Console.WriteLine($"allowing {options.ConcurrentConnectionsAllowed} connections");
            MaxConnectionsAllowed = options.ConcurrentConnectionsAllowed;
            
            var reportTimer = new System.Timers.Timer(10000);
            reportTimer.Elapsed += delegate { OutputReport(); };
            reportTimer.Start();

            // clear contents of file
            new FileRepo().Clear();
            

            new Thread(new ThreadStart(ThreadConnection)).Start();
            while (ServerRunning)
            {

                while (queue.Any())
                {


                    Connection connection;
                    var success = queue.TryDequeue(out connection);
                    if (success)
                    {
                        //Thread InstanceCaller = new Thread(
                        //    new ThreadStart(connection.SaveNumbers));
                        //InstanceCaller.Name = "SaveNumbers";
                        //InstanceCaller.Start();
                        connection.SaveNumbers();
                        if (connection.BubbleKillAll)
                        {
                            // loop through all connections and kill
                            foreach (var conn in queue)
                            {
                                conn.Abort();
                            }
                            ServerRunning = false;
                        }
                        else
                        {
                            if (!connection.Finished)
                                queue.Enqueue(connection);
                        }
                    }
                }

                // is this necessary if using thread-safe collections?
                // will this work with the true param added to reset?
                //reset.WaitOne();
            }
        }
        

        private void ThreadConnection()
        {
            TcpListener listener;
            IPAddress localAdd = IPAddress.Parse(IP_ADDRESS);
            listener = new TcpListener(localAdd, PORT_NUMBER);
            listener.Start();
            Console.WriteLine("Listening");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                //Console.WriteLine($"max allowed: {MaxConnectionsAllowed}");
                //Console.WriteLine($"queue count: {queue.Count}");
                if (queue.Count < MaxConnectionsAllowed)
                {
                    queue.Enqueue(new Connection(client));
                    //reset.Set();
                }
                else
                {
                    // TODO: anything "client" should be in the Connection class
                    client.Close();
                }
            }
        }


        private static void OutputReport()
        {
            var numberCollection = NumberCollectionSingleton.Instance;
            Console.WriteLine($"Received {numberCollection.GetLatestNewUniques()} unique numbers, {numberCollection.GetLatestDuplicateCount()} duplicates. Unique total: {numberCollection.GetTotalCount()} ");
        }
    }
}
