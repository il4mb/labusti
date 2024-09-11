
using System.Net.Sockets;
using System.Text;


namespace Remote
{
    class Program
    {
        public static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Remote Desktop Labor\r\nUsage:\tremote.exe \\\\hostname [[ command | [-c|-x] @file]");
                return;
            }

            Dictionary<string, string> options = CommandInterpreter.ParseCommandLine(String.Join(" ", args));

            CommandInterpreter.ProcessOptions(options);

            return;

            try
            {
                // Create a TcpClient instance and connect to the server
                TcpClient client = new TcpClient("127.0.0.1", 4040);

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                for (int i = 0; i < args.Length; i++) {
                    string arg = args[i];
                    
                }

                // client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}