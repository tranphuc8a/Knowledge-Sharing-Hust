using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra định dạng số điện thoại
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class PhoneValidator : RegexValidator
    {
        public PhoneValidator()
        {
            Pattern = PHONE_REGEX_PATTERN;
        }
    }
}
