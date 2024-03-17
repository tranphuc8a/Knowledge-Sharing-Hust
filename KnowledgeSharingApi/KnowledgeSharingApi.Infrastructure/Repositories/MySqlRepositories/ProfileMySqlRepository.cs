using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class ProfileMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Profile>(dbContext), IProfileRepository
    {
        public virtual Task<Profile?> GetByEmail(string email)
        {
            IQueryable<Profile> result = 
                from profile in DbContext.ViewUsers
                where profile.Email == email
                select profile;
            return result.FirstOrDefaultAsync();
        }

        public virtual Task<Profile?> GetByUserId(string userId)
        {
            IQueryable<Profile> query = 
                from profile in DbContext.ViewUsers
                where profile.UserId.ToString() == userId
                select profile;
            return query.FirstOrDefaultAsync();
        }

        public virtual Task<Profile?> GetByUsername(string username)
        {
            IQueryable<Profile> result = 
                from profile in DbContext.ViewUsers
                where profile.Username == username
                select profile;
            return result.FirstOrDefaultAsync();
        }

        public Task<Profile?> GetByUsernameOrUserId(string unOruid)
        {
            IQueryable<Profile> result =
                from profile in DbContext.ViewUsers
                where profile.Username == unOruid || profile.UserId.ToString() == unOruid
                select profile;
            return result.FirstOrDefaultAsync();
        }

        //public virtual Task<PaginationResponseModel<Profile>> Search(string uid, string searchKey, int? limit, int? offset)
        //{
        //    // Không bị blockee và blocked
        //    IEnumerable<Guid> lstBlockeeUid = DbContext.UserRelations
        //        .Where(relation => relation.UserRelationType == EUserRelationType.Block && relation.ReceiverId.ToString() == uid)
        //        .Select(relation => relation.SenderId)
        //        .AsEnumerable();
        //    IEnumerable<Guid> lstBlockedUid = DbContext.UserRelations
        //        .Where(relation => relation.UserRelationType == EUserRelationType.Block && relation.SenderId.ToString() == uid)
        //        .Select(relation => relation.ReceiverId)
        //        .AsEnumerable();

        //    // Tiêu chí tìm kiếm: Username, Họ tên, Số điện thoại
        //    IEnumerable<Profile> listed =
        //        from profile in DbContext.Profiles
        //        join user in DbContext.Users on profile.UserId equals user.UserId
        //        let commonLength = Algorithm.LongestCommonSubsequence(profile.FullName, searchKey)
        //        let minLength = Math.Ceiling(0.75 * profile.FullName.Length)
        //        let reliability = commonLength * Math.Sqrt(profile.FullName.Length)
        //        where commonLength > minLength && user.Role != UserRoles.Banned // không bị khóa
        //                && !lstBlockedUid.Contains(user.UserId)     // không bị chặn bởi uid
        //                && !lstBlockeeUid.Contains(user.UserId)     // không chặn uid
        //        orderby commonLength descending
        //        select profile;

        //    listed = listed.AsEnumerable();
        //    limit ??= listed.Count();
        //    IEnumerable<Profile> res = listed.Skip(offset ?? 0).Take((int) limit);
        //    return Task.FromResult(new PaginationResponseModel<Profile>()
        //    {
        //        Limit = (int) limit,
        //        Offset = offset ?? 0,
        //        Total = listed.Count(),
        //        Results = res
        //    });
        //}
    }
}
