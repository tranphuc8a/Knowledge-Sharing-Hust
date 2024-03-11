using KnowledgeSharingApi.Domains.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Kiểm tra tiền phải là số dương
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class MoneyValidator : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            if (int.TryParse(value.ToString(), out int money))
            {
                if (money < 0)
                {
                    throw new ValidatorException(ErrorMessage ?? GetType().Name);
                }
            }
            return true;
        }
    }
}