using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories
{
    public interface IProfileRepository : IRepository<Profile>
    {
        public Task<Profile?> GetByUserId(string userId);
        public Task<Profile?> GetByUsername(string username);
        public Task<Profile?> GetByEmail(string email);
    }
}
