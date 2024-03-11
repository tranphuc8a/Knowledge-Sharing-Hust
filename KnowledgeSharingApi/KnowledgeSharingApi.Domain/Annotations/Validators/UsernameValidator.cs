using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra định dạng username theo username regex pattern
    /// </summary>
    /// Created: PhucTV (11/3/24)
    /// Modified: None
    public class UsernameValidator : RegexValidator
    {
        public UsernameValidator()
        {
            Pattern = RegexValidator.USERNAME_REGEX_PATTERN;
        }
    }
}
