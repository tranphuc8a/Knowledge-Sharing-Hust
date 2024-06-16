using KnowledgeSharingApi.Domains.Algorithms;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlUserIteractionRepositories
{
    public class ProfileMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Profile>(dbContext), IProfileRepository
    {
        public virtual Task<ViewUserProfile?> GetByEmail(string email)
        {
            IQueryable<ViewUserProfile> result =
                from profile in DbContext.ViewUserProfiles
                where profile.Email == email
                select profile;
            return result.FirstOrDefaultAsync();
        }

        public virtual Task<ViewUserProfile?> GetByUserId(Guid userId)
        {
            IQueryable<ViewUserProfile> query =
                from profile in DbContext.ViewUserProfiles
                where profile.UserId == userId
                select profile;
            return query.FirstOrDefaultAsync();
        }

        public virtual Task<ViewUserProfile?> GetByUsername(string username)
        {
            IQueryable<ViewUserProfile> result =
                from profile in DbContext.ViewUserProfiles
                where profile.Username == username
                select profile;
            return result.FirstOrDefaultAsync();
        }

        public Task<ViewUserProfile?> GetByUsernameOrUserId(string unOruid)
        {
            IQueryable<ViewUserProfile> result =
                from profile in DbContext.ViewUserProfiles
                where profile.Username == unOruid || profile.UserId.ToString() == unOruid
                select profile;
            return result.FirstOrDefaultAsync();
        }

        //public virtual Task<PaginationResponseModel<Profile>> Search(Guid uid, string searchKey, int? limit, int? offset)
        //{
        //    // Không bị blockee và blocked
        //    List<Guid> lstBlockeeUid = DbContext.UserRelations
        //        .Where(relation => relation.UserRelationType == EUserRelationType.Block && relation.ReceiverI == uid)
        //        .Select(relation => relation.SenderId)
        //        .AsEnumerable();
        //    List<Guid> lstBlockedUid = DbContext.UserRelations
        //        .Where(relation => relation.UserRelationType == EUserRelationType.Block && relation.SenderI == uid)
        //        .Select(relation => relation.ReceiverId)
        //        .AsEnumerable();

        //    // Tiêu chí tìm kiếm: Username, Họ tên, Số điện thoại
        //    List<Profile> listed =
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
        //    List<Profile> res = listed.Skip(offset ?? 0).Take((int) limit);
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
