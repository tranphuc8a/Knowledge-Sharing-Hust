using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra trường role phải có giá trị trong ENUM
    /// </summary>
    /// Created: PhucTV (15/3/24)
    /// Modified: None
    public class RoleValidator : ValidationAttribute
    {
        public static readonly List<string> ListRole = [UserRoles.Admin, UserRoles.User, UserRoles.Banned];
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            string role = (string)value;
            if (!ListRole.Contains(role))
            {
                throw new ValidatorException(ErrorMessage ?? GetType().Name);
            }
            return true;
        }
    }
}
