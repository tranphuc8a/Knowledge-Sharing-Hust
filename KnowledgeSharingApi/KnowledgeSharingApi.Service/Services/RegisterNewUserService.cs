﻿using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

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

        protected override EVerifyCodeType VerifyCodeType { get; set; } = EVerifyCodeType.Register;
        protected override string EmailSubject { get; set; } = resourceFactory.GetResponseResource().RegistrationEmailSubject();

        public override async Task CheckEmailIsValid(string? email)
        {
            // Check email is valid
            await base.CheckEmailIsValid(email);

            // Check email is not in database
            User? user = await userRepository.GetByEmail(email!);
            if (user != null) throw new ValidatorException(responseResource.ExistedUser());
        }


        public override async Task<ServiceResult> Action(ActiveCodeModel codeModel)
        {
            await CheckActiveCode(codeModel);

            ServiceResult insertFailure = ServiceResult.ServerError(responseResource.AddNewUserSuccess());

            // Kiểm tra một lần nữa email chưa được đăng ký
            // ...

            // cast codemodel to ActiveCodeRegisterModel
            if (codeModel is ActiveCodeRegisterModel model)
            {
                // Create new user to add
                User user = new()
                {
                    Email = model.Email!,
                    Username = model.Username!,
                    Role = UserRoles.User
                };

                ServiceResult res = await AddNewUser(user, model.Password!, model.FullName!);

                if (res.IsSuccess)
                {
                    // Thành công, xóa cache
                    // ...
                }
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
        {
            ValidatorException insertFailedException = new(responseResource.AddNewUserFailure());

            // Begin transaction
            UnitOfWork.BeginTransaction();
            UnitOfWork.RegisterRepository(userRepository).RegisterRepository(profileRepository);

            try
            {
                // Step 1: Insert user to db
                string? insertId = await userRepository.Insert(user)
                    ?? throw insertFailedException;

                // Step 2: Update password
                int rowEffects = await userRepository.UpdatePassword(user.Username, password);
                if (rowEffects <= 0) throw insertFailedException;

                // Step 3: Insert Profile
                string? profileId = await profileRepository.Insert(new Profile()
                {
                    UserId = user.UserId,
                    FullName = fullName
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

        protected override string EmailContent(string verifyCode)
        {
            return responseResource.RegistrationEmailContent(verifyCode);
        }
    }
}