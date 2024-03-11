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
    /// Validate chiều dài của mã code
    /// Phải có độ dài từ 6 đến 20 ký tự
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class CodeLengthValidator : ValidationAttribute
    {
        public const int MIN_CODE_LENGTH = 6;
        public const int MAX_CODE_LENGTH = 20;

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            string val = (string)value;
            if (val.Length < MIN_CODE_LENGTH || val.Length > MAX_CODE_LENGTH)
            {
                throw new ValidatorException(ErrorMessage ?? GetType().Name);
            }
            return true;
        }
    }
}
