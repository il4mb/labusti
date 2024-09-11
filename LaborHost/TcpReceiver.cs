using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Host
{
    internal class TcpReceiver
    {
        static async Task Main(string[] args)
        {
            int port = 4050;
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            try
            {
                listener.Start();
                Console.WriteLine($"Listening for connections on port {port}...");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected.");

                    // Handle the client connection in a new task
                    _ = Task.Run(() => HandleClientAsync(client));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                listener.Stop();
            }
        }

        static async Task HandleClientAsync(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {receivedText}");

                    // Optionally, send a response back to the client
                    byte[] response = Encoding.UTF8.GetBytes("Message received");
                    await stream.WriteAsync(response, 0, response.Length);
                }
            }

            Console.WriteLine("Client disconnected.");
        }
    }
}
