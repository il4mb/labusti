

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Client.Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener listener = new(IPAddress.Any, 4021);

            try
            {
                listener.Start();
                Console.WriteLine("Client STARTED.");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected.");

                    //await callback(client); // Handle each client connection asynchronously
                }
            } catch(Exception e)
            {

            } finally
            {
                Console.WriteLine("Client STOPED");
                listener.Stop();
            }
        }
    }
}