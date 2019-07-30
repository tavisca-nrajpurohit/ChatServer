using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    class Server
    {
        static void Main(string[] args)
        {
            RunServer();
        }

        static void RunServer()
        {
            int port = 8080;
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = ipHost.AddressList[1];

            Console.WriteLine("SERVER MACHINE");
            Console.WriteLine("Server Ip Address : " + iPAddress);
            Console.WriteLine("Port : " + port);

            IPEndPoint endPoint = new IPEndPoint(iPAddress,port);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(endPoint);
            listener.Listen(10);

            try
            {
                Socket clientSocket = listener.Accept();
                Console.WriteLine("Connected to Client " + clientSocket.RemoteEndPoint);

                while (true)
                {
                    byte[] receivedBytes = new Byte[1024];
                    int receivedBytesLength = clientSocket.Receive(receivedBytes);
                    String data = Encoding.ASCII.GetString(receivedBytes, 0, receivedBytesLength);
                    Console.Write("Client : ");
                    Console.WriteLine(data);

                    if (data.IndexOf("exit") > -1)
                        break;

                    string input;
                    Console.Write("Server : ");
                    input = Console.ReadLine();

                    byte[] sentMessage = Encoding.ASCII.GetBytes(input);
                    int sentMessageLength = clientSocket.Send(sentMessage);
                    if (input.IndexOf("exit") > -1)
                        break;
                }
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch
            {
                Console.WriteLine("Error: Unable to connect to Client!");
            }




        }
    }
}


