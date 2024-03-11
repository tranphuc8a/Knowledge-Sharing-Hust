using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts
{
    public interface IDbContext : IDisposable
    {
        public IDbConnection Connection { get; set; }

        public IDbTransaction? Transaction { get; set; }
    }
}
