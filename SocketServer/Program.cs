using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    class Program
    {
        const int port = 25000;
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress addr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(addr, port);
                server.Start();
                Console.WriteLine("Server started on address: {0}, port: {1}", addr.ToString(), port);

                byte[] buffer = new byte[256];
                String data = null;

                while (true)
                {
                    ChatClient client = new ChatClient(server.AcceptTcpClient());
                    Console.WriteLine("New client connected");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("Hit enter to continue");
            Console.Read();
        }
    }
}

