using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
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
        protected override EVerifyCodeType VerifyCodeType { get; set; } = EVerifyCodeType.ForgotPassword;
        protected override string EmailSubject { get; set; } = resourceFactory.GetResponseResource().ForgotPasswordEmailSubject();

        public override async Task CheckEmailIsValid(string? email)
        {
            // Kiểm tra định dạng email
            await base.CheckEmailIsValid(email);

            // Kiểm tra email phải đã tồn tại trong cơ sở dữ liệu rồi
            _ = await userRepository.GetByEmail(email!)
                ?? throw new ValidatorException(responseResource.NotExistUser());
        }


        public override async Task<ServiceResult> Action(ActiveCodeModel codeModel)
        {
            // Verify code
            await CheckActiveCode(codeModel);

            // Kiểm tra codeModel phải là ActiveCodeForgotPassModel
            if (codeModel is ActiveCodeForgotPasswordModel model)
            {
                // Kiểm tra lại email phải tồn tại trong db
                User? user = await userRepository.GetByEmail(model.Email!);
                if (user == null)
                {
                    return ServiceResult.ServerError(responseResource.NotExistUser());
                }

                // OK, thực hiện reset password
                return await ResetPassword(user.Username, model.NewPassword!);
            }

            throw new NotMatchTypeException();
        }


        /// <summary>
        /// Thực hiện cập nhật mật khẩu mới cho username
        /// </summary>
        /// <param name="username"> Username cần cập nhật mật khẩu </param>
        /// <param name="newPassword"> Mật khẩu mới </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
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

        protected override string EmailContent(string verifyCode)
        {
            return responseResource.ForgotPasswordEmailContent(verifyCode);
        }
    }
}
