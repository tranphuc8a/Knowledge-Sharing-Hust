using KnowledgeSharingApi.Domains.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.DbContexts
{
    public class MySqlDbContext(IConfiguration configuration) : DbContext
    {
        //private readonly IConfiguration Configuration = configuration;
        private readonly string ConnectionString = configuration["ConnectionStrings:MariaDb"] ?? String.Empty;
        private readonly string MySqlVersion = "10.4.32-MariaDB";
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(MySqlVersion));
        }
    }
}
