using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Validate một tài khoản ngân hàng
    /// Phải là chuỗi số từ 4 đến 20 kí tự
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class BankAccountValidator : RegexValidator
    {
        public BankAccountValidator()
        {
            Pattern = RegexValidator.BANKACCOUNT_REGEX_PATTERN;
        }
    }
}
