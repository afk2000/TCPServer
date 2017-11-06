using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace TCPClient
{
    class Program
    {
        private const string IP_ADDRESS = "127.0.0.1";
        private const int PORT_NUMBER = 4000;
        private List<TcpClient> tcpClients;
        static void Main(string[] args)
        {
            // this will server as your integration tests:

            //// accepts 5 concurrent connections// close all connections
            try
            {
                var items = GetRandomFormattedList();
                //new Program().SendBytes("000000001|000000002|000000003");
                //new Program().SendBytes("000000000|terminate");
                new Program().SendBatch(5, items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: You should be able to have these 5 connections");
            }
        }
        

        private void SendBatch(int connectionsToCreate, List<string> items)
        {
            tcpClients = new List<TcpClient>();
            var itemCountToSend = 1000;
            for (var i=0; i<= connectionsToCreate; i++)
            {
                try
                {
                    var intRange = items.GetRange(i * itemCountToSend, itemCountToSend);
                    var stringRange = String.Join("|", intRange);
                    SendBytes(stringRange);
                    //SendBytes("terminate");
                } catch(Exception ex)
                {
                    var m = ex.Message;
                    CloseAll();
                    throw ex;
                }
            }
            Thread.Sleep(1000);
            CloseAll();
            SendBatch(connectionsToCreate, items);
            //SendBytes("terminate");
        }

        private void SendBytes(string textToSend)
        {
            TcpClient client = new TcpClient(IP_ADDRESS, PORT_NUMBER);
            //tcpClients.Add(client);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

            try
            {
                //---send the text---
                Console.WriteLine("Sending : " + textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                //client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CloseAll();
                throw ex;
            }
        }

        private void CloseAll()
        {
            foreach (var tcp in tcpClients)
            {
                try
                {
                    tcp.Close();
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }
            }
        }

        #region data generation shiz

        private static List<int> GetRandomIntList()
        {
            var intList = new List<int>();

            var rnd = new Random();
            for (var i = 0; i < 9999999; i++)
            {
                var thisRnd = rnd.Next(1, 999999999);
                intList.Add(thisRnd);
            }
            return intList;
        }

        private static List<string> GetRandomFormattedList()
        {
            var intList = GetRandomIntList();
            var formattedList = intList.Select(i => $"{i:D9}").ToList();
            return formattedList;
        }
        //private static List<string> GetRandomFormattedQueue()
        //{
        //    var intList = GetRandomIntList();
        //    var formattedList = intList.Select(i => $"{i:D9}").ToList();
        //    return formattedList;
        //}

        #endregion

    }
}
