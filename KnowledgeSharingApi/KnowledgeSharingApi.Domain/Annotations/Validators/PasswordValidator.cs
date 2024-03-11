using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra định dạng mật khẩu theo password regex pattern
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public class PasswordValidator : RegexValidator
    {
        public PasswordValidator()
        {
            Pattern = RegexValidator.PASSWORD_REGEX_PATTERN;
        }
    }
}
