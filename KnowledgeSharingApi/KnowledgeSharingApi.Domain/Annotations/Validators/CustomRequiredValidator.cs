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
    /// Kiểm tra một trường không được null hoặc rỗng
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class CustomeRequiredValidator : ValidationAttribute
    {
        public CustomeRequiredValidator() { }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                throw new ValidatorException(ErrorMessage ?? GetType().Name);
            }
            if (string.IsNullOrEmpty(value.ToString()))
            {
                throw new ValidatorException(ErrorMessage ?? GetType().Name);
            }
            return true;
        }

    }
}
