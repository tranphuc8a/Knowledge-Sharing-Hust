using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        protected readonly IUserRepository UserRepository;
        public UserService(IResourceFactory resourceFactory, IUserRepository userRepository) : base(resourceFactory)
        {
            UserRepository = userRepository;
            ResponseTableName = EntityResource.User();
        }

        protected override IUserRepository GetRepository()
        {
            return UserRepository;
        }

        protected override void ValidateBeforeInsert(User entity)
        {
            throw new NotImplementedException();
        }

        protected override void ValidateBeforeUpdate(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
