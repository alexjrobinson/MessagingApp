using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiClient
{
    class Program
    { 
        const int port = 25000;
        static bool _setup = false;
        static string message;
        static string nickname;
        static byte[] data = new byte[1024];
        private static IPAddress server;
        private static TcpClient client = new TcpClient();

        static void Main(string[] args)
        {
            try
            {
                connect();
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    sendMessage();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            Console.Read();
        }

        private static void connect()
        {
            Console.WriteLine("--- Welcome to socket based chat client ---");
            server = IPAddress.Parse("127.0.0.1");
            client.Connect(server.ToString(), port);
            Console.WriteLine("Connected via IP Address {0} on port {1}", server.ToString(), port);

            if (_setup == false)
            {
                setNickname();
                _setup = true;
            }

            Thread ctThread = new Thread(receiveMessage);
            ctThread.Start();
        }

        private static void setNickname()
        {
            Console.Write("Please enter a nickname: ");
            nickname = Console.ReadLine();
            byte[] nickData = new byte[1024];
            nickData = Encoding.ASCII.GetBytes(nickname);

            NetworkStream stream = client.GetStream();
            stream.Write(nickData, 0, nickData.Length);
            Console.WriteLine("You have entered the chat as {0}", nickname);
        }

        private static void receiveMessage()
        {
            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] readBuffer = new byte[1024];
                int numOfBytes = 0;
                numOfBytes = stream.Read(readBuffer, 0, readBuffer.Length);
                if (numOfBytes == 0)
                {
                    return;
                }
                byte[] data = new byte[numOfBytes];
                Array.Copy(readBuffer, data, numOfBytes);
                string message = Encoding.ASCII.GetString(data);
                Console.WriteLine(message);
            }
        }

        private static void sendMessage()
        {
            NetworkStream stream = client.GetStream();
            message = Console.ReadLine();
            string time = DateTime.Now.ToLongTimeString();
            message = $"[{time}] {nickname}: {message}";
            data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}