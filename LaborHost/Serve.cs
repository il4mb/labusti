using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    internal class Serve
    {

        public static async Task Start(int port, Func<TcpClient, Task> callback)
        {
            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            TcpListener listener = new TcpListener(IPAddress.Any, port);

            try
            {
                listener.Start();
                Debug.WriteLine($"Server started on port {port}");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Debug.WriteLine("Client connected.");
                    await callback(client); // Handle each client connection asynchronously
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
