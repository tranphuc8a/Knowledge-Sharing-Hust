using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class ForgotPasswordService(
            ICache cache,
            IResourceFactory resourceFactory,
            IEmail emailService,
            IUserRepository userRepository
    ) : BaseVerifyByEmailService(cache, resourceFactory, emailService), IForgotPasswordService
    {
        protected readonly IUserRepository userRepository = userRepository;

        public override async Task CheckEmailIsValid(string? email)
        {
            // Kiểm tra định dạng email
            await base.CheckEmailIsValid(email);

            // Kiểm tra email phải đã tồn tại trong cơ sở dữ liệu rồi
            _ = await userRepository.GetByEmail(email!)
                ?? throw new ValidatorException(responseResource.NotExistUser());
        }


        public override async Task<ServiceResult> Action(VerifyCodeModel codeModel)
        {
            // Verify code
            ServiceResult res = await CheckVerifyCode(codeModel);
            if (!res.IsSuccess) return res;

            // Kiểm tra codeModel phải là VerifyCodeForgotPassModel
            if (codeModel is VerifyCodeForgotPasswordModel model)
            {
                User? user = await userRepository.GetByEmail(model.Email!);
                if (user == null)
                {
                    return ServiceResult.ServerError(responseResource.NotExistUser());
                }

                return await ResetPassword(user.Username, model.NewPassword!);
            }

            throw new NotMatchTypeException();
        }


        protected virtual async Task<ServiceResult> ResetPassword(string username, string newPassword)
        {
            try
            {
                int updated = await userRepository.UpdatePassword(username, newPassword);
                if (updated <= 0)
                {
                    return ServiceResult.ServerError(responseResource.ResetPasswordFailure());
                }
                
                return ServiceResult.Success(responseResource.ChangePasswordSuccess());
            }
            catch (Exception error)
            {
                return ServiceResult.ServerError(
                        responseResource.ResetPasswordFailure(),
                        error.Message,
                        error.ToString()
                    );
            }
        }
    }
}
