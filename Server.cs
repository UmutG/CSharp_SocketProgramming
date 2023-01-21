using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
            Console.WriteLine("Server-side initializing...");
            // Since we work on the local PC, give host as localhost => 127.0.0.1
            IPHostEntry host = Dns.GetHostEntry("localhost");
            // We have only one IP address. So, access to first address on the host list.
            IPAddress ipAddress = host.AddressList[0];
            // Create an end point with given port number.
            IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, 11000);

            try
            {
                // Create a socket that will listen TCP protocol.
                Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Using Bind function, bound listener to given end-point.
                serverSocket.Bind(serverEndPoint);
                // Listen function will limit server how many request it can handles.
                serverSocket.Listen(10);

                Console.WriteLine("Waiting connection");
                // If client sends connection request, accept it.
                Socket handler = serverSocket.Accept();

                // Incoming data from the client.
                string data = null;
                byte[] bytes = null;

                // Until incoming data ends, get all data and concatanete to data string.
                while (true)
                {
                    // Each data can be as big as 64 bytes.
                    bytes = new byte[64];
                    // Assign incoming bytes to bytesRec variable.
                    int bytesRec = handler.Receive(bytes);
                    // Since given data is on byte format, convert it to string via ASCII format.
                    // bytes, 0, bytesRec => bytes array, starting index, last index
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    // If there is no data to get, terminate the loop.
                    if(data.Length > -1)
                    {
                        break;
                    }
                }

                // Write out the given text from client.
                Console.WriteLine("Client text is: \n{0}", data);

                // Echo the given message to client side with given timestamp.
                byte[] msg = Encoding.ASCII.GetBytes(data + " - " + DateTime.Now.ToString("HH:mm:ss"));
                handler.Send(msg);
                // Close all sockets and handler.
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }
    }
}
