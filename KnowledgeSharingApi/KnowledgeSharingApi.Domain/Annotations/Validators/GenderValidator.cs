using KnowledgeSharingApi.Domains.Enums;
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
    /// Kiểm tra trường giới tính phải có giá trị trong ENUM
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class GenderValidator : ValidationAttribute
    {
        private static readonly List<EGender> _ListGender = [EGender.Male, EGender.Female, EGender.Other];
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            if (int.TryParse(value.ToString(), out int gender))
            {
                if (!_ListGender.Contains((EGender)gender))
                {
                    throw new ValidatorException(ErrorMessage ?? GetType().Name);
                }
            }
            return true;
        }
    }
}
