using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class ChatClient
    {
        public static Hashtable AllClients = new Hashtable();

        private TcpClient _client;
        private string _clientIP;
        private string _clientNick;

        private byte[] data;
        private bool ReceivedNick = true;

        public ChatClient(TcpClient client)
        {
            _client = client;

            _clientIP = client.Client.RemoteEndPoint.ToString();

            AllClients.Add(_clientIP, this);

            data = new byte[_client.ReceiveBufferSize];
            client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(_client.ReceiveBufferSize), ReceiveMessage, null);
        }

        public void ReceiveMessage(IAsyncResult AR)
        {
            int bytesRead;
            try
            {
                lock (_client.GetStream())
                {
                    bytesRead = _client.GetStream().EndRead(AR);
                }

                if (bytesRead < 1)
                {
                    AllClients.Remove(_clientIP);
                    Broadcast(_clientNick + " has left the chat.");
                    return;
                }
                else
                {
                    string messageReceived = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);

                    if (ReceivedNick)
                    {
                        _clientNick = messageReceived;

                        Broadcast(_clientNick + " has joined the chat.");
                        ReceivedNick = false;
                    }
                    else
                    {
                        Broadcast(messageReceived);
                    }
                }

                lock (_client.GetStream())
                {
                    _client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(_client.ReceiveBufferSize), ReceiveMessage, null);
                }
            }
            catch (Exception e)
            {
                AllClients.Remove(_clientIP);
                Broadcast(_clientNick + " has left the chat.");
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                System.Net.Sockets.NetworkStream stream;
                lock (_client.GetStream())
                {
                    stream = _client.GetStream();
                }

                byte[] bytesToSend = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(bytesToSend, 0, bytesToSend.Length);
                stream.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Broadcast(string message)
        {
            Console.WriteLine(message);
            foreach (DictionaryEntry c in AllClients)
            {
                ((ChatClient)(c.Value)).SendMessage(message);
            }
        }
    }
}
