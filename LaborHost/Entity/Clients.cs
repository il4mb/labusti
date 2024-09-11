using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Entity
{

    class ClientContext : DbContext
    {
        DbSet<Client> Clients { get; set; }

        public string DbPath { get; }

        public ClientContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "clients.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }


    public class Client
    {
        public int ClientId { get; set; }
        public required string Hostname { get; set; }
        public required string Label { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

    }
}
