using Dapper;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlUserIteractionRepositories
{
    public class SessionMySqlRepository(IDbContext dbContext, IUserRepository userRepository)
        : BaseMySqlRepository<Session>(dbContext), ISessionRepository
    {
        protected readonly IUserRepository userRepository = userRepository;
        public async Task<int> DeleteByUsername(string username)
        {
            string sqlCommand = $"Delete from Session where " +
                                $"UserId in (Select UserId from User where Username = @username); ";
            int rowEffect = await Connection.ExecuteAsync(sqlCommand, new { username }, Transaction);
            return rowEffect;
        }
    }
}
