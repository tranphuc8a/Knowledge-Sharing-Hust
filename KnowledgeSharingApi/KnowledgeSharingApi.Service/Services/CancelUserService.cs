﻿using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace KnowledgeSharingApi.Services.Services
{
    public class CancelUserService(
        ICache cache,
        IResourceFactory resourceFactory,
        IUserRepository userRepository,
        IEmail emailService,
        IUnitOfWork unitOfWork
    ) : BaseVerifyByEmailService(cache, resourceFactory, emailService), ICancelUserService
    {
        protected IUnitOfWork UnitOfWork = unitOfWork;
        protected override EVerifyCodeType VerifyCodeType { get; set; } = EVerifyCodeType.CancelUser;
        protected override string EmailSubject { get; set; } = resourceFactory.GetResponseResource().CancelUserEmailSubject();
        protected readonly IUserRepository UserRepository = userRepository;

        public override async Task<ServiceResult> Action(ActiveCodeModel codeModel)
        {
            // Step 1. Verify code
            await CheckActiveCode(codeModel);

            // Step 2. Kiểm tra lại email phải có tồn tại
            User? user = await UserRepository.GetByEmail(codeModel.Email!);
            if (user == null) return ServiceResult.BadRequest(responseResource.NotExistUser());

            // Step 3. Thực hiện xóa tài khoản back ground
            _ = DoDeleteUserBackground(user!);

            // Step 4. Trả về thành công
            return ServiceResult.Success(responseResource.CancelUserSuccess());
        }

        protected override string EmailContent(string verifyCode)
        {
            return responseResource.CancelUserEmailContent(verifyCode, VerifyCodeExpiredInMinutes);
        }

        protected virtual Task DoDeleteUserBackground(User user)
        {
            try
            {
                // Begin transaction and register repository
                UnitOfWork.BeginTransaction();
                UnitOfWork.RegisterRepository(UserRepository);

                // Xóa lần lượt các phần tử của user
                // Xóa comment ...

                // Tạm thời chưa xử lý tới, throw ra exception
                throw new Exception();

            }
            catch (Exception ex)
            {
                // Có lỗi xảy ra, rollback
                UnitOfWork.RollbackTransaction();

                // Console.log(Errorr)
                Console.WriteLine(ex.ToString());

                // Gửi email thất bại tới user
                emailService.Send(
                    toEmail: user.Email,
                    subject: responseResource.CancelUserRespomseEmailSubject(),
                    content: responseResource.CancelUserFailureProcessResponseEmailContent(user.Username)
                );
            }
            return Task.CompletedTask;
        }
    }
}