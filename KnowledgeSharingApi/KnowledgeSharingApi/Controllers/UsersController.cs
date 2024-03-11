using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User>
    {
        #region Fields and Constructor
        private readonly IUserRepository _UserRepository;
        private readonly IUserService _UserService;
        public UsersController(
            IUserRepository userRepository,
            IUserService customerGroupService,
            IResourceFactory resourceFactory) : base(resourceFactory)
        {
            _UserRepository = userRepository;
            _UserService = customerGroupService;
            ResponseTableName = _EntityResource.User();
        }
        #endregion


        #region Template methods steps
        protected override IUserRepository GetRepository()
        {
            return _UserRepository;
        }

        protected override IUserService GetService()
        {
            return _UserService;
        }
        #endregion

    }
}
