using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
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
            IQueryable<Profile> 
                result = from profile in DbContext.Profiles
                         join user in DbContext.Users
                            on profile.UserId equals user.UserId
                         where user.Email == email
                         select profile;
            return result.FirstOrDefaultAsync();
        }

        public virtual Task<Profile?> GetByUserId(string userId)
        {
            IQueryable<Profile>
                query = from profile in DbContext.Profiles
                        where profile.UserId.ToString() == userId
                        select profile;
            return query.FirstOrDefaultAsync();
        }

        public virtual Task<Profile?> GetByUsername(string username)
        {
            IQueryable<Profile>
                result = from profile in DbContext.Profiles
                         join user in DbContext.Users
                            on profile.UserId equals user.UserId
                         where user.Username == username
                         select profile;
            return result.FirstOrDefaultAsync();
        }
    }
}
