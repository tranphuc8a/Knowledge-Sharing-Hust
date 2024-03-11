using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.DbContexts
{
    public class MySqlDbContext : DbContext, IDbContext
    {
        //private readonly IConfiguration Configuration = configuration;
        private readonly string ConnectionString;
        private readonly string MySqlVersion = "10.4.32-MariaDB";
        public DbSet<User> Users { get; set; }
        public IDbConnection Connection { get; set; }
        public IDbTransaction? Transaction { get; set; }

        public MySqlDbContext(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:MariaDb"] ?? String.Empty;
            Connection = new MySqlConnection(ConnectionString);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(MySqlVersion));
        }

        public override void Dispose()
        {
            Connection?.Dispose();
            Transaction?.Dispose();
            base.Dispose();
        }
    }
}
