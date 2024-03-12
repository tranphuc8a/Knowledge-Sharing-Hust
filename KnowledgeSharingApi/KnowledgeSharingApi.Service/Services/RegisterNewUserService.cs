using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
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
        protected IUserRepository userRepository = userRepository;
        protected IProfileRepository profileRepository = profileRepository;
        protected IUnitOfWork UnitOfWork = unitOfWork;

        public override async Task CheckEmailIsValid(string? email)
        {
            // Check email is valid
            await base.CheckEmailIsValid(email);

            // Check email is not in database
            User? user = await userRepository.GetByEmail(email!);
            if (user != null) throw new ValidatorException(responseResource.ExistedUser());
        }


        public override async Task<ServiceResult> Action(VerifyCodeModel codeModel)
        {
            ServiceResult insertFailure = ServiceResult.ServerError(responseResource.AddNewUserSuccess());
            // Verify codeModel
            ServiceResult res = await CheckVerifyCode(codeModel);
            if (!res.IsSuccess) return res;

            // cast codemodel to VerifyCodeRegisterModel
            if (codeModel is VerifyCodeRegisterModel model)
            {
                // Begin transaction
                UnitOfWork.BeginTransaction();
                UnitOfWork.RegisterRepository(userRepository).RegisterRepository(profileRepository);

                // Add new User and return user with new UserId
                User user = new()
                {
                    Email = model.Email!,
                    Username = model.Username!,
                    Role = UserRoles.User
                };

                ValidatorException insertFailedException = new(responseResource.AddNewUserFailure());
                try
                {
                    // Step 1: Create user
                    string? insertId = await userRepository.Insert(user)
                        ?? throw insertFailedException;

                    // Step 2: Update password
                    int rowEffects = await userRepository.UpdatePassword(model.Username!, model.Password!);
                    if (rowEffects <= 0) throw insertFailedException;

                    // Step 3: Create Profile
                    string? profileId = await profileRepository.Insert(new Profile()
                    {
                        UserId = user.UserId,
                        FullName = model.FullName!
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
                return ServiceResult.Success(responseResource.AddNewUserSuccess());
            }

            // failure: Server error
            return insertFailure;
        }
    }
}
