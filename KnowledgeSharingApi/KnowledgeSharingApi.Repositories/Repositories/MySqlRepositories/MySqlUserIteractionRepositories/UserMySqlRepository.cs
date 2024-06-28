using Dapper;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlUserIteractionRepositories
{
    public class UserMySqlRepository(IDbContext _dbContext, IEncrypt encrypt)
        : BaseMySqlRepository<User>(_dbContext), IUserRepository
    {
        protected readonly IEncrypt encrypt = encrypt;

        public virtual async Task<bool> CheckPassword(string username, string password)
        {
            User? user = await GetByUsername(username);
            if (user == null) return false;

            return encrypt.VerifyPassword(username, password, user.HashPassword);
        }

        public virtual async Task<bool> CheckPassword(string username, string password, string hashPassword)
        {
            bool isPasswordMatch = encrypt.VerifyPassword(username, password, hashPassword);
            return await Task.FromResult(isPasswordMatch);
        }

        public virtual async Task<int> UpdatePassword(string username, string newPassword)
        {
            string hashPassword = encrypt.HashPassword(username, newPassword);
            DateTime currentTime = DateTime.UtcNow;
            string modifiedBy = username;
            string sqlCommand = "update User set HashPassword = @hashPassword, ModifiedTime = @currentTime, ModifiedBy = @modifiedBy " +
                                "where Username = @username;";
            int res = await Connection.ExecuteAsync(sqlCommand, new { username, hashPassword, currentTime, modifiedBy }, Transaction);
            return res;
        }

        #region Getter
        public virtual async Task<List<ViewUserProfile>> GetDetail()
        {
            return await DbContext.ViewUserProfiles.ToListAsync();
        }

        public virtual async Task<List<ViewUserProfile>> GetDetail(PaginationDto pagination)
        {
            return await ApplyPagination(DbContext.ViewUserProfiles, pagination).ToListAsync();
        }

        public virtual Task<ViewUserProfile?> GetDetail(Guid userId)
        {
            var query = from user in DbContext.ViewUserProfiles
                        where user.UserId == userId
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual Task<ViewUserProfile?> GetDetailByEmail(string email)
        {
            var query = from viewUser in DbContext.ViewUserProfiles
                        where viewUser.Email == email
                        select viewUser;
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual Task<ViewUserProfile?> GetDetailByUsername(string username)
        {
            var query = from user in DbContext.ViewUserProfiles
                        where user.Username == username
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual Task<ViewUserProfile?> GetDetailByUsernameOrUserId(string unOruid)
        {
            var query = from user in DbContext.ViewUserProfiles
                        where user.Username == unOruid || user.UserId.ToString() == unOruid
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual async Task<Dictionary<Guid, ViewUserProfile?>> GetDetail(Guid[] userIds)
        {
            Dictionary<Guid, ViewUserProfile?> res = userIds.Distinct().ToDictionary(id => id, id => (ViewUserProfile?)null);

            List<ViewUserProfile> users = await DbContext.ViewUserProfiles
                .Where(user => userIds.Contains(user.UserId))
                .ToListAsync();

            foreach (var item in users)
            {
                res[item.UserId] = item;
            }
            return res;
        }

        public virtual async Task<List<T>> GetDetail<T>(Expression<Func<ViewUserProfile, T>> projector)
        {
            return await DbContext.ViewUserProfiles.Select(projector).ToListAsync();
        }

        public virtual async Task<List<T>> GetDetail<T>(PaginationDto pagination, Expression<Func<ViewUserProfile, T>> projector)
        {
            return await ApplyPagination(DbContext.ViewUserProfiles.Select(projector), pagination).ToListAsync();
        }

        public virtual Task<T?> GetDetail<T>(Guid userId, Expression<Func<ViewUserProfile, T>> projector)
        {
            var query = DbContext.ViewUserProfiles.Where(u => u.UserId == userId).Select(projector);
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual Task<T?> GetDetailByEmail<T>(string email, Expression<Func<ViewUserProfile, T>> projector)
        {
            var query = DbContext.ViewUserProfiles.Where(u => u.Email == email).Select(projector);
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual Task<T?> GetDetailByUsername<T>(string username, Expression<Func<ViewUserProfile, T>> projector)
        {
            var query = DbContext.ViewUserProfiles.Where(u => u.Username == username).Select(projector);
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual Task<T?> GetDetailByUsernameOrUserId<T>(string unOruid, Expression<Func<ViewUserProfile, T>> projector)
        {
            var query = DbContext.ViewUserProfiles.Where(user => user.Username == unOruid || user.UserId.ToString() == unOruid)
                        .Select(projector);
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual async Task<Dictionary<Guid, T?>> GetDetail<T>(Guid[] userIds, Expression<Func<ViewUserProfile, T>> projector)
        {
            Dictionary<Guid, T?> res = userIds.Distinct().ToDictionary(id => id, id => default(T));

            Dictionary<Guid, T> users = await DbContext.ViewUserProfiles
                .Where(user => userIds.Contains(user.UserId))
                .ToDictionaryAsync(
                    item => item.UserId,
                    projector.Compile()
                );

            foreach (var item in users)
            {
                res[item.Key] = item.Value;
            }
            return res;
        }

        public virtual Task<User?> GetByEmail(string email)
        {
            var query = from user in DbContext.Users
                        where user.Email == email
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual Task<User?> GetByUsername(string username)
        {
            var query = from user in DbContext.Users
                        where user.Username == username
                        select user;
            return Task.FromResult(query.FirstOrDefault());
        }

        public virtual async Task<PaginationResponseModel<T>> GetByUserId<T>(Guid userId, PaginationDto pagination) where T : Entity
        {
            string tableName = typeof(T).Name;
            // Check bảng T phải có trường UserId:
            _ = typeof(T).GetProperty("UserId") ?? throw new NotMatchTypeException();
            string sqlCommand = $"Select * from {tableName} " +
                                $"where UserId = @userId; ";
            PaginationResponseModel<T> res = await GetPagination<T>(sqlCommand, pagination, new { userId });
            return res;
        }

        public virtual async Task<PaginationResponseModel<T>> GetByUsername<T>(string username, PaginationDto pagination) where T : Entity
        {
            string tableName = typeof(T).Name;
            // Check bảng T phải có trường UserId:
            _ = typeof(T).GetProperty("UserId") ?? throw new NotMatchTypeException();
            string sqlCommand = $"Select * from {tableName} " +
                                $"where UserId in (Select UserId from User where Username = @username); ";
            PaginationResponseModel<T> res = await GetPagination<T>(sqlCommand, pagination, new { username });
            return res;
        }

        public virtual async Task<ViewUserProfile> CheckExistedUser(Guid userId, string errorMessage)
        {
            return (ViewUserProfile)((await DbContext.ViewUserProfiles.Where(user => user.UserId == userId).FirstOrDefaultAsync())?.Clone()
                ?? throw new NotExistedEntityException(errorMessage));
        }
        
        #endregion

        public virtual async Task<Guid?> RegisterUser(Guid userId, User user, string password, string fullName, string? avatar = null)
        {
            var transaction = await DbContext.BeginTransaction();
            try
            {
                user.HashPassword = encrypt.HashPassword(user.Username, password);
                user.Role = UserRoles.User;
                user.UserId = userId;

                Profile profile = new()
                {
                    CreatedBy = fullName,
                    CreatedTime = DateTime.UtcNow,
                    ProfileId = Guid.NewGuid(),
                    UserId = userId,
                    FullName = fullName,
                    Avatar = avatar
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

        // override delete user:
        public override Task<int> Delete(Guid userId)
        {
            // Tam thoi chua cho xoa user
            return Task.FromResult(0);
        }

        public virtual async Task PromoteToAdmin(Guid uid)
        {
            User u = await DbContext.Users.FindAsync(uid) ?? throw new Exception("User not found");
            u.Role = UserRoles.Admin;
            await DbContext.SaveChangesAsync();
        }
    }
}
