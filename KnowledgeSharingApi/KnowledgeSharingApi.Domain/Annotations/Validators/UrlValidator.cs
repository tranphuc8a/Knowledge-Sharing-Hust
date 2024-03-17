using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra chuỗi tuân theo định dạng là một đường dẫn URL
    /// </summary>
    /// Created: PhucTV (15/3/24)
    /// Modified: None
    public class UrlValidator : RegexValidator
    {
        public UrlValidator()
        {
            Pattern = URL_REGEX_PATTERN;
        }
    }
}
