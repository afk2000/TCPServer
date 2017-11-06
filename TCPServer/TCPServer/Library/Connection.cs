using System;
using System.Net.Sockets;
using System.Text;

namespace TCPServer.Library
{

    public sealed class Connection
    {
        readonly TcpClient client;
        public bool BubbleKillAll = false;

        public Connection(TcpClient client)
        {
            this.client = client;
        }

        public void SaveNumbers()
        {
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            var fileRepo = new FileRepo();
            try
            {


                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                if (bytesRead > 0)
                {
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    var arrReceived = NumberUtility.GetStringArray(dataReceived);

                    foreach (var str in arrReceived)
                    {
                        if (NumberUtility.IsTerminate(str))
                        {
                            // terminate all
                            BubbleKillAll = true;
                        }

                        if (NumberUtility.IsValidFormat(str))
                        {
                            fileRepo.Append(str);
                        }
                        else
                        {
                            // kill this connection
                            Abort();
                        }
                    }
                }
            } catch (Exception ex)
            {
                var m = ex.Message;
            }

        }

        public void Abort()
        {
            // message client?
            byte[] abortMessage = Encoding.ASCII.GetBytes("Thread aborted by server.");
            int abortMessageLength = abortMessage.Length;
            client.GetStream().Write(abortMessage, 0, abortMessageLength);
            client.Close();
        }


        public bool Finished
        {
            get
            {
                return !client.Connected;
            }
        }
    }
}
