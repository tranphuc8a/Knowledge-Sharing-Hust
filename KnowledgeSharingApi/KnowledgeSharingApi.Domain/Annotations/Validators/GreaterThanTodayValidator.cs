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
    /// Kiểm tra trường date không được lớn hơn ngày hôm nay
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class GreaterThanTodayValidator : ValidationAttribute
    {
        public GreaterThanTodayValidator() { }

        /// <summary>
        /// Annotation to Validate The Date is Smaller than Today
        /// </summary>
        /// <param name="value"></param>
        /// <returns> true/false </returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: none
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            if (DateTime.TryParse(value.ToString(), out DateTime date))
            {
                // compare date with today
                if (DateTime.UtcNow < date)
                {
                    throw new GreaterThanTodayException(ErrorMessage ?? GetType().Name);
                }
            }
            return true;
        }

    }
}
