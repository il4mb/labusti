
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Resources;

namespace Host
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        string AppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Labor");

        public App()
        {
            initializeDirectory();

            _ = Serve.Start(4010, callback: ResponseTCPAsync);
            _ = Serve.Start(4020, callback: ResponseHTTP);
        }



        void initializeDirectory()
        {
            if (!Directory.Exists(AppPath))
            {
                Directory.CreateDirectory(AppPath);
            }

            Uri RootWeb = new Uri("pack://application:,,,/Host;component/Web");

            Debug.WriteLine(RootWeb.LocalPath);

        }



        async Task ResponseTCPAsync(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.WriteLine($"Received: {receivedText}");

                    // Echo the message back to the client
                    await stream.WriteAsync(buffer, 0, bytesRead);
                }
            }

            Debug.WriteLine("Client disconnected.");
        }



        async Task ResponseHTTP(TcpClient client)
        {

            IPEndPoint remoteIpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            Debug.WriteLine("IP : " + remoteIpEndPoint.Address);

            Uri RootWeb = new Uri("pack://application:,,,/Host;component/Web");

            using (NetworkStream stream = client.GetStream())
            {
      
                byte[] buffer = new byte[4096]; // Increase buffer size for larger requests
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received request: {request}");

                // Extract requested resource from the request (assuming it's a GET request)
                string[] requestLines = request.Split("\r\n");
                string requestLine    = requestLines[0];
                string[] requestParts = requestLine.Split(' ');
                string requestedFile  = (requestParts[1] == "/" ? "/index.html": requestParts[1]);

                // Build resource URI for embedded resource
                string resourceUri = $"pack://application:,,,/Host;component/Web{requestedFile}";
                Debug.WriteLine(resourceUri);

                try
                {
                    // Extract the resource from the pack URI
                    StreamResourceInfo resourceInfo = GetResourceStream(new Uri(resourceUri));

                    if (resourceInfo != null)
                    {
                        // Get the file extension to determine if it's a text or binary file
                        string extension   = Path.GetExtension(requestedFile).ToLower();
                        string contentType = GetContentType(extension);  // Define the content type

                        if (IsBinaryFile(extension))
                        {
                            // Binary file (e.g., images, fonts, etc.)
                            using (MemoryStream ms = new MemoryStream())
                            {
                                resourceInfo.Stream.CopyTo(ms);
                                byte[] fileContent = ms.ToArray();

                                // HTTP response headers and body for binary files
                                string httpResponse = $"HTTP/1.1 200 OK\r\n" +
                                                      $"Content-Type: {contentType}\r\n" +
                                                      $"Content-Length: {fileContent.Length}\r\n" +
                                                      "\r\n";

                                byte[] responseHeaders = Encoding.UTF8.GetBytes(httpResponse);
                                await stream.WriteAsync(responseHeaders, 0, responseHeaders.Length);
                                await stream.WriteAsync(fileContent, 0, fileContent.Length);
                            }
                        }
                        else
                        {
                            // Text-based file (e.g., HTML, CSS, JS)
                            using (StreamReader reader = new StreamReader(resourceInfo.Stream))
                            {
                                string fileContent = await reader.ReadToEndAsync();

                                // HTTP response headers and body for text files
                                string httpResponse = $"HTTP/1.1 200 OK\r\n" +
                                                      $"Content-Type: {contentType}; charset=UTF-8\r\n" +
                                                      $"Content-Length: {Encoding.UTF8.GetByteCount(fileContent)}\r\n" +
                                                      "\r\n" +
                                                      fileContent;

                                byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse);
                                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                            }
                        }
                    }
                    else
                    {
                        // Return 404 if the file doesn't exist
                        string notFoundResponse = "HTTP/1.1 404 Not Found\r\n" +
                                                  "Content-Type: text/html; charset=UTF-8\r\n" +
                                                  "\r\n" +
                                                  "<html><body><h1>404 - File Not Found</h1></body></html>";

                        byte[] responseBytes = Encoding.UTF8.GetBytes(notFoundResponse);
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accessing resource: {ex.Message}");

                    // Return 500 if there was an error processing the request
                    string errorResponse = "HTTP/1.1 500 Internal Server Error\r\n" +
                                           "Content-Type: text/html; charset=UTF-8\r\n" +
                                           "\r\n" +
                                           "<html><body><h1>500 - Internal Server Error</h1></body></html>";

                    byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
            }

            Console.WriteLine("Client disconnected.");
        }


        string GetContentType(string extension)
        {
            switch (extension)
            {
                case ".html": return "text/html";
                case ".css": return "text/css";
                case ".js": return "application/javascript";
                case ".png": return "image/png";
                case ".jpg": case ".jpeg": return "image/jpeg";
                case ".gif": return "image/gif";
                case ".woff": return "font/woff";
                case ".woff2": return "font/woff2";
                case ".ttf": return "font/ttf";
                case ".svg": return "image/svg+xml";
                default: return "application/octet-stream";  // Fallback for unknown types
            }
        }

        // Determines if the file is a binary file based on the extension
        bool IsBinaryFile(string extension)
        {
            return extension == ".png" || extension == ".jpg" || extension == ".jpeg" ||
                   extension == ".gif" || extension == ".woff" || extension == ".woff2" ||
                   extension == ".ttf" || extension == ".svg";

        }

        public static void MoveFilesRecursively(string sourceDir, string targetDir)
        {
            // Ensure the target directory exists
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Move files in the current directory
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string targetFilePath = Path.Combine(targetDir, fileName);

                // Move the file
                File.Move(file, targetFilePath);
                Console.WriteLine($"Moved file: {file} -> {targetFilePath}");
            }

            // Recursively move subdirectories
            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(dir);
                string targetSubDir = Path.Combine(targetDir, dirName);

                // Recursive call to move files in subdirectories
                MoveFilesRecursively(dir, targetSubDir);
            }

            // Optionally, delete the empty source directory
            // Directory.Delete(sourceDir, true);
        }
    }

}