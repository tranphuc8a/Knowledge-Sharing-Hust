using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.UserProfileModels
{
    public class UpdateUserModel
    {
        [EmailValidator(ErrorMessage = ViConstantResource.EMAIL_FORMAT)]
        public string? Email { get; set; }

        [UsernameValidator(ErrorMessage = ViConstantResource.USERNAME_FORMAT)]
        public string? Username { get; set; }

        [PasswordValidator(ErrorMessage = ViConstantResource.PASSWORD_FORMAT)]
        public string? Password { get; set; }

        [RoleValidator(ErrorMessage = ViConstantResource.ROLE_FORMAT)]
        public string? Role { get; set; }
    }
}
