using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LaborClient
{
    class Program
    {

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
            var dict = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {   
                string arg = args[i];
                if(arg.StartsWith("-") && args.Length > i+1)
                {
                    dict.Add(arg.Substring(1), args[i+1]);
                    i++;
                }
            }

            if (dict.Count <= 0)
            {
                Console.WriteLine("Tidak ada perintah di berikan!.");
            } else
            {
                for(int i = 0; i < dict.Count; i++)
                {
                    string key = dict.Keys.ElementAt(i);

                    Console.WriteLine(Handle(key, dict.GetValueOrDefault(key)));
                }
            }



        }
    }
}

//internal class Program
//{
//    [DllImport("user32.dll")]
//    private static extern IntPtr GetDesktopWindow();

//    [DllImport("user32.dll")]
//    private static extern IntPtr GetWindowDC(IntPtr hWnd);

//    [DllImport("gdi32.dll")]
//    private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation rop);

//    [DllImport("gdi32.dll")]
//    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

//    [DllImport("gdi32.dll")]
//    private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

//    [DllImport("gdi32.dll")]
//    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

//    [DllImport("gdi32.dll")]
//    private static extern bool DeleteObject(IntPtr hObject);

//    [DllImport("gdi32.dll")]
//    private static extern bool DeleteDC(IntPtr hdc);

//    public static Bitmap CaptureFullScreen()
//    {
//        // Get the desktop window handle
//        IntPtr desktopWnd = GetDesktopWindow();
//        IntPtr desktopDC = GetWindowDC(desktopWnd);
//        IntPtr memDC = CreateCompatibleDC(desktopDC);


//        // Get the screen dimensions
//        int screenWidth = (int)(Screen.PrimaryScreen.Bounds.Width * 1.25);
//        int screenHeight = (int)(Screen.PrimaryScreen.Bounds.Height * 1.25);

//        // Create a compatible bitmap for the screen dimensions
//        IntPtr hBitmap = CreateCompatibleBitmap(desktopDC, screenWidth, screenHeight);
//        IntPtr oldBitmap = SelectObject(memDC, hBitmap);

//        // Perform bit-block transfer (copy screen to bitmap)
//        BitBlt(memDC, 0, 0, screenWidth, screenHeight, desktopDC, 0, 0, CopyPixelOperation.SourceCopy);

//        // Create a .NET Bitmap from the HBITMAP
//        Bitmap bitmap = Image.FromHbitmap(hBitmap);

//        // Clean up GDI objects
//        SelectObject(memDC, oldBitmap);
//        DeleteObject(hBitmap);
//        DeleteDC(memDC);
//        DeleteDC(desktopDC);

//        return bitmap;
//    }

//    public static Bitmap ResizeImage(Bitmap originalImage, int targetHeight)
//    {
//        int targetWidth = (int)((float)originalImage.Width / originalImage.Height * targetHeight);
//        Bitmap resizedImage = new Bitmap(targetWidth, targetHeight);

//        using (Graphics g = Graphics.FromImage(resizedImage))
//        {
//            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
//            g.DrawImage(originalImage, 0, 0, targetWidth, targetHeight);
//        }

//        return resizedImage;

//    }

//    public static void Main(string[] args)
//    {

//        // Capture the entire screen
//        //Bitmap screenshot = CaptureFullScreen();
//        //Bitmap resize = ResizeImage(screenshot, 240);
//        //resize.Save(@"C:\temp\bitblt_screenshot.jpg", ImageFormat.Jpeg);

//        //Console.WriteLine("Full screen captured with BitBlt, including taskbar!");
//    }
//}