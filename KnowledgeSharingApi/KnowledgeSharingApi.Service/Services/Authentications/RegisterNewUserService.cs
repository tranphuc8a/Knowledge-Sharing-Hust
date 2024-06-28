using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces.Authentications;

namespace KnowledgeSharingApi.Services.Services.Authentications
{
    public class RegisterNewUserService(
            ICache cache,
            IResourceFactory resourceFactory,
            IEmail emailService,
            IUserRepository userRepository,
            IProfileRepository profileRepository,
            IUnitOfWork unitOfWork
        ) : BaseVerifyByEmailService(cache, resourceFactory, emailService), IRegisterNewUserService
    {
        protected IUserRepository UserRepository = userRepository;
        protected IProfileRepository profileRepository = profileRepository;
        protected IUnitOfWork UnitOfWork = unitOfWork;

        protected override EVerifyCodeType VerifyCodeType { get; set; } = EVerifyCodeType.Register;
        protected override string EmailSubject { get; set; } = resourceFactory.GetResponseResource().RegistrationEmailSubject();


        public override async Task CheckEmailIsValid(string? email)
        {
            // Check email is valid
            await base.CheckEmailIsValid(email);

            // Check email is not in database
            User? user = await UserRepository.GetByEmail(email!);
            if (user != null) throw new ValidatorException(ResponseResource.ExistedUser());
        }


        protected override async Task<ServiceResult> ActionHook(ActiveCodeModel codeModel)
        {
            ServiceResult insertFailure = ServiceResult.ServerError(ResponseResource.AddNewUserSuccess());

            // Kiểm tra một lần nữa email chưa được đăng ký
            User? tempUser = await UserRepository.GetByEmail(codeModel.Email!);
            if (tempUser != null)
                return ServiceResult.BadRequest(ResponseResource.ExistedUser());

            // cast codemodel to ActiveCodeRegisterModel
            if (codeModel is ActiveCodeRegisterModel model)
            {
                // Kiểm tra username chưa được đăng ký
                tempUser = await UserRepository.GetByUsername(model.Username!);
                if (tempUser != null)
                    return ServiceResult.BadRequest(ResponseResource.ExistedUser());

                // Create new user to add
                User user = new()
                {
                    Email = model.Email!,
                    Username = model.Username!,
                    Role = UserRoles.User
                };

                ServiceResult res = await AddNewUser(user, model.Password!, model.Username!);

                return res;
            }

            // failure: Server error
            return insertFailure;
        }

        /// <summary>
        /// Thực hiện thêm mới user vào database
        /// </summary>
        /// <param name="user">user muốn thêm</param>
        /// <param name="password"> Mật khẩu người dùng </param>
        /// <param name="fullName"> Tên đầy đủ </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        protected virtual async Task<ServiceResult> AddNewUser(User user, string password, string fullName)
        {       // --> Tốt nhất nên viết vào repository
            ValidatorException insertFailedException = new(ResponseResource.AddNewUserFailure());

            // Begin transaction
            UnitOfWork.BeginTransaction();
            UnitOfWork.RegisterRepository(UserRepository).RegisterRepository(profileRepository);

            try
            {
                // Step 1: Insert user to db
                Guid? insertId = await UserRepository.Insert(user)
                    ?? throw insertFailedException;

                // Step 2: Update password
                int rowEffects = await UserRepository.UpdatePassword(user.Username, password);
                if (rowEffects <= 0) throw insertFailedException;

                // Step 3: Insert Profile
                Guid? profileId = await profileRepository.Insert(new Profile()
                {
                    UserId = user.UserId,
                    FullName = fullName,
                    CreatedBy = fullName,
                    CreatedTime = DateTime.UtcNow
                }) ?? throw insertFailedException;

                // Step 4: Commit
                UnitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                UnitOfWork.RollbackTransaction();
                throw;
            }

            // return success
            return ServiceResult.Success(ResponseResource.AddNewUserSuccess());
        }

        protected override string EmailContent(string verifyCode)
        {
            return ResponseResource.RegistrationEmailContent(verifyCode);
        }
    }
}
