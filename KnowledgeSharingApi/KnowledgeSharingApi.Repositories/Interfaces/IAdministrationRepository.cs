using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.Repositories
{
    public interface IAdministrationRepository 
    {
        IDbConnection Connection { get; set; }
        IDbTransaction? Transaction { get; set; }
        IDbContext DbContext { get; set; }

        /// <summary>
        /// Thuc hien truy van sql
        /// </summary>
        /// <param name="query"> noi dung cau truy van</param>
        /// <returns></returns>
        /// Created PhucTV (20/5/24)
        /// Modified None
        Task<object> QueryByConnection(string query);
        Task<object> QueryByDbContext(string query);
    }
}
