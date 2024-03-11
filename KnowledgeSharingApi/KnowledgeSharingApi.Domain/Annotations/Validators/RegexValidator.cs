using KnowledgeSharingApi.Domains.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Lớp abstract kiểm tra một string phải có định dạng theo Pattern Regex
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public abstract class RegexValidator : ValidationAttribute
    {
        //public const string PHONE_REGEX_PATTERN             = @"^(1[ \-\+]{0,3}|\+1[ -\+]{0,3}|\+1|\+)?((\(\+?1-[2-9][0-9]{1,2}\))|(\(\+?[2-8][0-9][0-9]\))|(\(\+?[1-9][0-9]\))|(\(\+?[17]\))|(\([2-9][2-9]\))|([ \-\.]{0,3}[0-9]{2,4}))?([ \-\.][0-9])?([ \-\.]{0,3}[0-9]{2,4}){2,3}$";
        public const string PHONE_REGEX_PATTERN = @"^[\+\(\s.\-\/\d\)]{1,30}$";
        public const string IDENTITYCARD_REGEX_PATTERN = @"^\d{9}$|^\d{12}$";
        public const string EMAIL_REGEX_PATTERN = @"^[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9_-]+)*\.[a-zA-Z]{2,}$";

        public const string BANKACCOUNT_REGEX_PATTERN = @"^\d{4,20}$";
        public const string USERNAME_REGEX_PATTERN = @"^[a-zA-Z][a-zA-Z0-9_]{3,15}$";
        public const string PASSWORD_REGEX_PATTERN = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[a-zA-Z\d@$!%*?&]{8,}$";

        protected string? Pattern { get; set; }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            if (!Regex.IsMatch(value.ToString()!, Pattern ?? String.Empty))
            {
                throw new ValidatorException(ErrorMessage ?? GetType().Name);
            }
            return true;
        }
    }
}
