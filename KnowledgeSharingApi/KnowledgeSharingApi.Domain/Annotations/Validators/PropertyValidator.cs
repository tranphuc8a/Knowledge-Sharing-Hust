using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Validators
{
    /// <summary>
    /// Validate dữ liệu
    /// </summary>
    /// Created: PhucTV (18/1/24)
    /// Modified: None
    public static class PropertyValidator
    {
        /// <summary>
        /// Check if date is greater than today
        /// </summary>
        /// <param name="date"> Ngày cần kiểm tra </param>
        /// <returns> true - greater, false - smaller </returns>
        /// Created: PhucTV (18/1/24)
        /// Modified: None
        public static bool IsGreaterThanToday(DateTime? date)
        {
            if (date == null)
            {
                return false;
            }
            return date > DateTime.Now;
        }


        /// <summary>
        /// Kiểm tra đúng định dạng email
        /// </summary>
        /// <param name="email"> chuỗi email cần kiểm tra </param>
        /// <returns> true - đúng format, false - sai format </returns>
        /// Created: PhucTV (18/1/24)
        /// Modified: None
        public static bool CheckFormatEmail(string? email)
        {
            return email == null || Regex.IsMatch(email, RegexValidator.EMAIL_REGEX_PATTERN);
        }


        /// <summary>
        /// Kiểm tra đúng định dạng phoneNumber
        /// </summary>
        /// <param name="phoneNumber"> chuỗi phoneNumber cần kiểm tra </param>
        /// <returns> true - đúng format, false - sai format </returns>
        /// Created: PhucTV (18/1/24)
        /// Modified: None
        public static bool CheckFormatPhoneNumber(string? phoneNumber)
        {
            return phoneNumber == null || Regex.IsMatch(phoneNumber, RegexValidator.PHONE_REGEX_PATTERN);
        }


        /// <summary>
        /// Kiểm tra đúng định dạng gender
        /// </summary>
        /// <param name="gender"> gender cần kiểm tra </param>
        /// <returns> true - đúng format, false - sai format </returns>
        /// Created: PhucTV (18/1/24)
        /// Modified: None

        public static bool CheckFormatGender(EGender? gender)
        {
            return gender == null || GenderValidator.ListGender.Contains((EGender)gender);
        }



        /// <summary>
        /// Kiểm tra đúng định dạng identityCardNumber
        /// </summary>
        /// <param name="identityCardNumber"> chuỗi identityCardNumber cần kiểm tra </param>
        /// <returns> true - đúng format, false - sai format </returns>
        /// Created: PhucTV (18/1/24)
        /// Modified: None
        public static bool CheckFormatIdentityCardNumber(string? identityCardNumber)
        {
            return identityCardNumber == null || Regex.IsMatch(identityCardNumber, RegexValidator.IDENTITYCARD_REGEX_PATTERN);
        }


        /// <summary>
        /// Kiểm tra đúng định dạng CodeLength (code là các mã khách hàng, mã nhân viên...)
        /// </summary>
        /// <param name="code"> chuỗi code cần kiểm tra </param>
        /// <returns> true - đúng format, false - sai format </returns>
        /// Created: PhucTV (18/1/24)
        /// Modified: None
        public static bool CheckFormatCodeLength(string? code)
        {
            return code == null || 
                   (code.Length >= CodeLengthValidator.MIN_CODE_LENGTH && code.Length <= CodeLengthValidator.MAX_CODE_LENGTH);
        }

        /// <summary>
        /// Kiểm tra đúng định dạng BankAccount
        /// </summary>
        /// <param name="bankAccount"> chuỗi số tài khoản cần kiểm tra </param>
        /// <returns> true - đúng format, false - sai format </returns>
        /// Created: PhucTV (18/1/24)
        /// Modified: None
        public static bool CheckFormatBankAccount(string? bankAccount)
        {
            return bankAccount == null || Regex.IsMatch(bankAccount, RegexValidator.BANKACCOUNT_REGEX_PATTERN);
        }


        /// <summary>
        /// Kiểm tra đúng định dạng username hay không
        /// </summary>
        /// <param name="username">username cần kiểm tra</param>
        /// <returns> true - đúng định dạng, false - sai định dạng</returns>
        /// Created: PhucTV (19/2/24)
        /// Modified: None
        public static bool CheckFormatUsername(string? username)
        {
            return username == null || Regex.IsMatch(username, RegexValidator.USERNAME_REGEX_PATTERN);
        }


        /// <summary>
        /// Kiểm tra đúng định dạng password hay không?
        /// </summary>
        /// <param name="password"></param>
        /// <returns> true - đúng định dạng, false - sai định dạng </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        public static bool CheckFormatPassword(string? password)
        {
            return password == null || Regex.IsMatch(password, RegexValidator.PASSWORD_REGEX_PATTERN);
        }


        /// <summary>
        /// Kiểm tra đúng định dạng role hay không?
        /// </summary>
        /// <param name="role"></param>
        /// <returns> true - đúng định dạng, false - sai định dạng </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        public static bool CheckFormatRole(string? role)
        {
            return role == null || RoleValidator.ListRole.Contains(role);
        }
    }
}
