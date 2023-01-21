using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        public static void StartClient()
        {
            Console.WriteLine("Client-side initializing...");
            byte[] bytes = new byte[64];

            try
            {
                // Since we work on the local PC, give host as localhost => 127.0.0.1
                IPHostEntry host = Dns.GetHostEntry("localhost");
                // We have only one IP address. So, access to first address on the host list.
                IPAddress ipAddress = host.AddressList[0];
                // Create an end point with given port number.
                IPEndPoint clienEndPoint = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    clientSocket.Connect(clienEndPoint);

                    Console.WriteLine("Connection established! => {0}", clientSocket.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("Hello");

                    // Send the data through the socket.
                    int bytesSent = clientSocket.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = clientSocket.Receive(bytes);
                    Console.WriteLine("Server Echo => {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }
    }
}
