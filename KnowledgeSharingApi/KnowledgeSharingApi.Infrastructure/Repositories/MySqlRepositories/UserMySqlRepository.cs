using Dapper;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class UserMySqlRepository(IDbContext _dbContext, IEncrypt encrypt)
        : BaseMySqlRepository<User>(_dbContext), IUserRepository
    {
        protected readonly IEncrypt encrypt = encrypt;

        public async Task<bool> CheckPassword(string username, string password)
        {
            User? user = await GetByUsername(username);
            if (user == null) return false;

            string hashPassword = encrypt.Sha256HashPassword(username, password);
            return hashPassword == user.HashPassword;
        }

        public async Task<int> UpdatePassword(string username, string newPassword)
        {
            string hashPassword = encrypt.Sha256HashPassword(username, newPassword);
            string sqlCommand = "update User set HashPassword = @hashPassword where Username = @username;";
            int res = await Connection.ExecuteAsync(sqlCommand, new { username, hashPassword }, Transaction);
            return res;
        }

        public async Task<IEnumerable<ViewUser>> GetDetail()
        {
            return await DbContext.ViewUsers.ToListAsync();
        }

        public async Task<IEnumerable<ViewUser>> GetDetail(int limit, int offset)
        {
            return await DbContext.ViewUsers.Skip(offset).Take(limit).ToListAsync();
        }

        public Task<ViewUser?> GetDetail(Guid userId)
        {
            var query = from user in DbContext.ViewUsers
                        where (user.UserId == userId)
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public Task<ViewUser?> GetDetailByEmail(string email)
        {
            var query = from viewUser in DbContext.ViewUsers
                        where (viewUser.Email == email)
                        select viewUser;
            return Task.FromResult(query.FirstOrDefault());
        }

        public Task<ViewUser?> GetDetailByUsername(string username)
        {
            var query = from user in DbContext.ViewUsers
                        where (user.Username == username)
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public Task<ViewUser?> GetDetailByUsernameOrUserId(string unOruid)
        {
            var query = from user in DbContext.ViewUsers
                        where (user.Username == unOruid || user.UserId.ToString() == unOruid)
                        select user;
            return Task.FromResult(query.FirstOrDefault());
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

        public async Task<PaginationResponseModel<T>> GetByUserId<T>(Guid userId, int limit, int offset) where T : Entity
        {
            string tableName = typeof(T).Name;
            // Check bảng T phải có trường UserId:
            _ = typeof(T).GetProperty("UserId") ?? throw new NotMatchTypeException();
            string sqlCommand = $"Select * from {tableName} " +
                                $"where UserId = @userId; ";
            PaginationResponseModel<T> res = await GetPagination<T>(sqlCommand, new { userId, limit, offset });
            res.Limit = limit;
            res.Offset = offset;
            return res;
        }

        public async Task<PaginationResponseModel<T>> GetByUsername<T>(string username, int limit, int offset) where T : Entity
        {
            string tableName = typeof(T).Name;
            // Check bảng T phải có trường UserId:
            _ = typeof(T).GetProperty("UserId") ?? throw new NotMatchTypeException();
            string sqlCommand = $"Select * from {tableName} " +
                                $"where UserId in (Select UserId from User where Username = @username); ";
            PaginationResponseModel<T> res = await GetPagination<T>(sqlCommand, new { username, limit, offset });
            res.Limit = limit;
            res.Offset = offset;
            return res;
        }

        public async Task<ViewUser> CheckExistedUser(Guid userId, string errorMessage)
        {
            return await DbContext.ViewUsers.Where(user => user.UserId == userId).FirstOrDefaultAsync()
                ?? throw new NotExistedEntityException(errorMessage);
        }

        public async Task<Dictionary<Guid, ViewUser?>> GetDetail(Guid[] userIds)
        {
            Dictionary<Guid, ViewUser?> res = userIds.ToDictionary(id => id, id => (ViewUser?)null);

            IEnumerable<ViewUser> users = await DbContext.ViewUsers
                .Where(user => userIds.Contains(user.UserId))
                .ToListAsync();

            foreach (var item in users)
            {
                res[item.UserId] = item;
            }
            return res;
        }

        public async Task<Guid?> RegisterUser(Guid userId, User user, string password, string fullName)
        {
            var transaction = await DbContext.BeginTransaction();
            try
            {
                user.HashPassword = encrypt.Sha256HashPassword(user.Username, password);
                user.Role = UserRoles.User;
                user.UserId = userId;

                Profile profile = new()
                {
                    CreatedBy = fullName,
                    CreatedTime = DateTime.Now,
                    ProfileId = Guid.NewGuid(),
                    UserId = userId,
                    FullName = fullName
                };

                DbContext.Users.Add(user);
                await DbContext.SaveChangesAsync();
                DbContext.Profiles.Add(profile);
                await DbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return userId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
