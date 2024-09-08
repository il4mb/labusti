using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;

namespace LaborClient
{
   
    internal class WebBlocker
    {

        public WebBlocker()
        {

            var proxyServer = new ProxyServer();

            // locally trust root certificate used by this proxy 
            // proxyServer.CertificateManager.TrustRootCertificate(true);

            // optionally set the Certificate Engine
            // Under Mono only BouncyCastle will be supported
            //proxyServer.CertificateManager.CertificateEngine = Network.CertificateEngine.BouncyCastle;

            proxyServer.BeforeRequest += OnRequest;
            proxyServer.BeforeResponse += OnResponse;
            proxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            proxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;


            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true)
            {
                // Use self-issued generic certificate on all https requests
                // Optimizes performance by not creating a certificate for each https-enabled domain
                // Useful when certificate trust is not required by proxy clients
                //GenericCertificate = new X509Certificate2(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "genericcert.pfx"), "password")
            };

            // Fired when a CONNECT request is received
            explicitEndPoint.BeforeTunnelConnectRequest += OnBeforeTunnelConnectRequest;

            // An explicit endpoint is where the client knows about the existence of a proxy
            // So client sends request in a proxy friendly manner
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();

            // Transparent endpoint is useful for reverse proxy (client is not aware of the existence of proxy)
            // A transparent endpoint usually requires a network router port forwarding HTTP(S) packets or DNS
            // to send data to this endPoint
            var transparentEndPoint = new TransparentProxyEndPoint(IPAddress.Any, 8001, true)
            {
                // Generic Certificate hostname to use
                // when SNI is disabled by client
                GenericCertificateName = "google.com"
            };

            proxyServer.AddEndPoint(transparentEndPoint);

            //proxyServer.UpStreamHttpProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };
            //proxyServer.UpStreamHttpsProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };

            foreach (var endPoint in proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            // Only explicit proxies can be set as system proxy!
            proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
            proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);

            // wait here (You can use something else as a wait function, I am using this as a demo)
            Console.Read();

            // Unsubscribe & Quit
            //explicitEndPoint.BeforeTunnelConnectRequest -= OnBeforeTunnelConnectRequest;
            //proxyServer.BeforeRequest -= OnRequest;
            //proxyServer.BeforeResponse -= OnResponse;
            //proxyServer.ServerCertificateValidationCallback -= OnCertificateValidation;
            //proxyServer.ClientCertificateSelectionCallback -= OnCertificateSelection;

            //proxyServer.Stop();

        }

        private static async Task OnResponse(object sender, SessionEventArgs e)
        {
            // read response headers
            var responseHeaders = e.HttpClient.Response.Headers;

            //if (!e.ProxySession.Request.Host.Equals("medeczane.sgk.gov.tr")) return;
            if (e.HttpClient.Request.Method == "GET" || e.HttpClient.Request.Method == "POST")
            {
                if (e.HttpClient.Response.StatusCode == 200)
                {
                    if (e.HttpClient.Response.ContentType != null && e.HttpClient.Response.ContentType.Trim().ToLower().Contains("text/html"))
                    {
                        byte[] bodyBytes = await e.GetResponseBody();
                        e.SetResponseBody(bodyBytes);

                        string body = await e.GetResponseBodyAsString();
                        e.SetResponseBodyString(body);
                    }
                }
            }

            if (e.UserData != null)
            {
                // access request from UserData property where we stored it in RequestHandler
                var request = (Request)e.UserData;
            }
        }

        private static async Task<Task> OnCertificateSelection(object sender, CertificateSelectionEventArgs e)
        {
            return Task.CompletedTask;
        }

        private static async Task<Task> OnCertificateValidation(object sender, CertificateValidationEventArgs e)
        {
            // set IsValid to true/false based on Certificate Errors
            if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                e.IsValid = true;

            return Task.CompletedTask;
        }

        private static async Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static async Task OnRequest(object sender, SessionEventArgs e)
        {
            Console.WriteLine(e.HttpClient.Request.Url);

            // Read request headers
            var requestHeaders = e.HttpClient.Request.Headers;

            var method = e.HttpClient.Request.Method.ToUpper();
            if (method == "POST" || method == "PUT" || method == "PATCH")
            {
                // Get/Set request body bytes
                byte[] bodyBytes = await e.GetRequestBody();
                e.SetRequestBody(bodyBytes);

                // Get/Set request body as string
                string bodyString = await e.GetRequestBodyAsString();
                e.SetRequestBodyString(bodyString);

                // Store request for later use
                e.UserData = e.HttpClient.Request;
            }

            // Filter URL
            if (e.HttpClient.Request.RequestUri.AbsoluteUri.Contains("google.com"))
            {
                string filePath = @"E:\www\Labor" + e.HttpClient.Request.RequestUri.PathAndQuery;

                if (File.Exists(filePath))
                {
                    string fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
                    string contentType = GetContentType(fileExtension);

                    // Set the Content-Type
                    e.HttpClient.Response.ContentType = contentType;

                    // Serve the file
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    e.Ok(fileBytes);
                    return;
                }

                string html = "<!DOCTYPE html>" +
                              "<html><body><h1>" +
                              "Website Blocked" +
                              "</h1>" +
                              "<p>Blocked by titanium web proxy.</p>" +
                              "</body></html>";

                try
                {
                    html = File.ReadAllText(@"E:\www\Labor\blocked.html");
                }
                catch (Exception) { }

                e.Ok(html);
            }
        }

        // Helper method to get content type based on file extension
        private static string GetContentType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".html":
                case ".htm":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".png":
                    return "image/png";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".json":
                    return "application/json";
                case ".pdf":
                    return "application/pdf";
                case ".xml":
                    return "application/xml";
                case ".zip":
                    return "application/zip";
                default:
                    return "application/octet-stream"; // Default content type
            }
        }
    }
}
