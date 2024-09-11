using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;

namespace Host.Entity
{


    internal class HostModel
    {
        protected string ip { get; set; }
        protected string label { get; set; }
        protected string media { get; set; }

        public HostModel(string label, string media)
        {
            this.ip    = GetLocalIPAddress();
            this.label = label;
            this.media = media;
        }


        private static string GetLocalIPAddress()
        {
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();
        }


        public string getIp()
        {
            return ip;
        }
        public string getLabel()
        {
            return label;
        }

        public string getMedia()
        {
            return media;
        }

        public void setIp(string ip)
        {
            this.ip = ip;
        } 
        public void setLabel(string label)
        {
            this.label = label;
        }

        public void setMedia(string media)
        {
            this.media = media;
        }
    }
}
