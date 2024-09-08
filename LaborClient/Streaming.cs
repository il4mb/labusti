using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace LaborClient
{
    internal class Streaming
    {
        private static bool status = false;
        public Streaming()
        {
            if (Streaming.status)
            {
                Console.WriteLine("Couldn't start streaming, instance has been started.");
                return;
            }

            const int port = 5000;
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Waiting for a connection...");
            Streaming.status = true;

            while (Streaming.status)
            {
                try
                {
                    using (TcpClient client = listener.AcceptTcpClient())
                    using (NetworkStream networkStream = client.GetStream())
                    {
                        Console.WriteLine("Client connected. Streaming...");

                        // Send HTTP response header for multipart content
                        string header = "HTTP/1.1 200 OK\r\n" +
                                        "Content-Type: multipart/x-mixed-replace; boundary=--boundary\r\n" +
                                        "Connection: keep-alive\r\n" +
                                        "\r\n";
                        byte[] headerBytes = Encoding.ASCII.GetBytes(header);
                        networkStream.Write(headerBytes, 0, headerBytes.Length);
                        networkStream.Flush();

                        while (Streaming.status)
                        {
                            try
                            {
                                using (Bitmap bitmap = new Bitmap((int)(Screen.PrimaryScreen.Bounds.Width * 1.25), (int)(Screen.PrimaryScreen.Bounds.Height * 1.25)))
                                using (Graphics graphics = Graphics.FromImage(bitmap))
                                {
                                    graphics.CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size);
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        // Resize the image if needed
                                        Bitmap resize = ResizeImage(bitmap, 240);
                                        resize.Save(ms, ImageFormat.Jpeg);
                                        byte[] imageBytes = ms.ToArray();

                                        // Create multipart boundary and headers for each frame
                                        string frameHeader = $"--boundary\r\n" +
                                                             "Content-Type: image/jpeg\r\n" +
                                                             $"Content-Length: {imageBytes.Length}\r\n" +
                                                             "\r\n";

                                        byte[] frameHeaderBytes = Encoding.ASCII.GetBytes(frameHeader);

                                        // Send the frame header and image
                                        networkStream.Write(frameHeaderBytes, 0, frameHeaderBytes.Length);
                                        networkStream.Write(imageBytes, 0, imageBytes.Length);

                                        // End frame with a blank line
                                        networkStream.Write(Encoding.ASCII.GetBytes("\r\n"), 0, 2);

                                        // Flush the stream to ensure the client receives data
                                        networkStream.Flush();
                                    }
                                }
                                // Control frame rate (1 fps)
                                Thread.Sleep(1000/15);
                            }
                            catch (IOException ioEx)
                            {
                                Console.WriteLine($"IOException during image capture or transmission: {ioEx.Message}");
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unexpected error: {ex.Message}");
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error with client connection: {ex.Message}");
                }
            }
        }


        public static Bitmap ResizeImage(Bitmap originalImage, int targetHeight)
        {
            int targetWidth = (int)((float)originalImage.Width / originalImage.Height * targetHeight);
            Bitmap resizedImage = new Bitmap(targetWidth, targetHeight);

            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, targetWidth, targetHeight);
            }

            return resizedImage;

        }
    
        
        public static void Stop()
        {
            Streaming.status = false;
            Console.WriteLine("Streaming has ben stoped");
        }
    }
}
