using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories
{
    public interface ISessionRepository : IRepository<Session>
    {
        /// <summary>
        /// Xóa phiên đăng nhập theo username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<int> DeleteByUsername(string username);
    }
}
