using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Client
{
    class Program
    {

        static string ROOT = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Labor.Client");


        static void StartProxy()
        {
            Console.WriteLine("Starting a proxy...");
            WebBlocker wb = new WebBlocker();
           

        }
        static void RestartProxy()
        {

            Console.WriteLine("Restart a proxy...");

        }
        static void StopProxy()
        {
            Console.WriteLine("Stopping a proxy...");

        }

        static void StreamingStart()
        {
            Console.WriteLine("Starting streaming...");
            Streaming sm = new Streaming();
        }
        static void StreamingStop()
        {
            Console.WriteLine("Stopping streaming...");
            Streaming.Stop();
        }

        static string Handle(string name, string value)
        {
            if (name == "proxy")
            {
                if (value == "start")
                {
                    StartProxy();
                }
                else if (value == "stop")
                {
                    StopProxy();
                }
                else if (value == "restart")
                {
                    RestartProxy();
                }
                else
                {
                    return "Proxy tidak mengenal parameter dengan nilai \"" + value + "\"";
                }
            }else if(name == "stream")
            {
                if (value == "start")
                {
                    StreamingStart();
                }
                else if (value == "stop")
                {
                    StreamingStop();
                } else
                {
                    return "Streaming tidak mengenal parameter dengan nilai \"" + value + "\"";
                }
            }
            else
            {

                return "Perintah \"" + name + "\" tidak diketahui!.";
            }
            return "";
        }


        public static void Main(string[] args)
        {


            try
            {
                // Define the registry key path
                string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

                // Open the registry key with write access
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        // Set the value for LocalAccountTokenFilterPolicy to 1
                        key.SetValue("LocalAccountTokenFilterPolicy", 1, RegistryValueKind.DWord);
                        Console.WriteLine("Registry key set successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Registry path not found.");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access denied. Please run the program as an administrator.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting registry key: {ex.Message}");
            }



            try
            {
                // Enable Remote Desktop
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true);
                if (key != null)
                {
                    key.SetValue("fDenyTSConnections", 0); // 0 enables Remote Desktop, 1 disables it
                    key.Close();
                }

                // Enable Network Level Authentication (NLA) for Remote Desktop (optional)
                key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", true);
                if (key != null)
                {
                    key.SetValue("UserAuthentication", 1); // 1 enables NLA, 0 disables it
                    key.Close();
                }

                Console.WriteLine("Remote Desktop has been enabled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting registry key: {ex.Message}");
            }



            try
            {
                // Command to enable Remote Desktop firewall rule
                Process.Start(new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = "advfirewall firewall set rule group=\"remote desktop\" new enable=yes",
                    Verb = "runas", // Run as administrator
                    UseShellExecute = true,
                    CreateNoWindow = true
                });

                Console.WriteLine("Firewall rule for Remote Desktop has been enabled.");

                // Enable Remote Administration firewall rule
                Process.Start(new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = "advfirewall firewall set rule group=\"Remote Administration\" new enable=yes",
                    Verb = "runas", // Run as administrator
                    UseShellExecute = true,
                    CreateNoWindow = true
                });


                Console.WriteLine("Firewall rule for Remote Administration has been enabled.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting firewall key: {ex.Message}");
            }



            try
            {
                // Path to the policy in the registry
                string registryPath = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";
                string valueName = "LimitBlankPasswordUse";

                // Set the value to 0 to allow remote access without a password
                Registry.SetValue(registryPath, valueName, 0, RegistryValueKind.DWord);

                Console.WriteLine("Remote access without password enabled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }



            // Restart the Remote Desktop service
            ServiceController sc = new ServiceController("TermService");
            sc.Stop();
            sc.WaitForStatus(ServiceControllerStatus.Stopped);
            sc.Start();
            sc.WaitForStatus(ServiceControllerStatus.Running);

            Console.WriteLine("Remote Desktop service restarted.");



            if (args.Length == 0)
            {
                Console.WriteLine("Remote Desktop Labor\r\nUsage:\r\nremote.exe [\\\\hostname] command");
                return;
            }

            try
            {
                // Create a TcpClient instance and connect to the server
                TcpClient client = new TcpClient(args[0], 4040);

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                // Send a message to the server (this could be any argument or data)
                string message = String.Join(" ", args.Where(e => e != args[0]).ToArray());
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: " + message);

                // Optionally, read a response from the server
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}