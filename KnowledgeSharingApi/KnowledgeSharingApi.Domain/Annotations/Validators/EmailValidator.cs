using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra email đúng định dạng Pattern
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class EmailValidator : RegexValidator
    {
        public EmailValidator()
        {
            Pattern = RegexValidator.EMAIL_REGEX_PATTERN;
        }
    }
}
