using Dapper;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class UserMySqlRepository(IDbContext dbContext, IEncrypt encrypt)
        : BaseMySqlRepository<User>(dbContext), IUserRepository
    {
        protected readonly IEncrypt encrypt = encrypt;

        public async Task<bool> CheckPassword(string username, string password)
        {
            User? user = await GetByUsername(username);
            if (user == null) return false;

            string hashPassword = encrypt.Sha256HashPassword(username, password);
            return hashPassword == user.HashPassword;
        }

        public Task<User?> GetByEmail(string email)
        {
            var query = from user in DbContext.Users
                        where (user.Email == email)
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public Task<User?> GetByUsername(string username)
        {
            var query = from user in DbContext.Users
                        where (user.Username == username)
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public async Task<int> UpdatePassword(string username, string newPassword)
        {
            string hashPassword = encrypt.Sha256HashPassword(username, newPassword);
            string sqlCommand = "update User set HashPassword = @hashPassword where Username = @username;";
            int res = await Connection.ExecuteAsync(sqlCommand, new { username, hashPassword}, Transaction);
            return res;
        }
    }
}
