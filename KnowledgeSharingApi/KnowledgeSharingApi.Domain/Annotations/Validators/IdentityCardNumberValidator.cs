using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra số căn cước công dân phải là chuỗi số độ dài 9 hoặc 12
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class IdentidyCardNumberValidator : RegexValidator
    {
        public IdentidyCardNumberValidator()
        {
            Pattern = RegexValidator.IDENTITYCARD_REGEX_PATTERN;
        }
    }
}
