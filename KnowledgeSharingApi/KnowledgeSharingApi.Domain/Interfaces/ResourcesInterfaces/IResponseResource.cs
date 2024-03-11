using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces
{
    /// <summary>
    /// ResourceInterfaces trả về của các Api response
    /// </summary>
    /// Created: PhucTV (10/1/24)
    /// Modified: None
    public interface IResponseResource
    {
        #region Insert
        /// <summary>
        /// ResourceInterfaces cho Insert
        /// </summary>
        /// <param name="entityName"> Tên bảng/Tên đối tượng </param>
        /// <returns> Chuỗi resource </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string InsertSuccess(string? entityName = null);
        string InsertFailure(string? entityName = null);
        string InsertMultiSuccess(string? entityName = null);
        string InsertMultiFalure(string? entityName = null);
        string InsertedSomeItems(string? entiName = null);
        #endregion


        #region Get

        /// <summary>
        /// ResourceInterfaces cho Get
        /// </summary>
        /// <param name="entityName"> Tên bảng/Tên đối tượng </param>
        /// <returns> Chuỗi resource </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string GetSuccess(string? entityName = null);
        string GetFailure(string? entityName = null);
        string GetMultiSuccess(string? entityName = null);
        string GetMultiFailure(string? entityName = null);
        #endregion

        #region Update

        /// <summary>
        /// ResourceInterfaces cho Update
        /// </summary>
        /// <param name="entityName"> Tên bảng/Tên đối tượng </param>
        /// <returns> Chuỗi resource </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string UpdateSuccess(string? entityName = null);
        string UpdateFailure(string? entityName = null);
        string UpdateMultiSuccess(string? entityName = null);
        string UpdateMultiFailure(string? entityName = null);
        #endregion


        #region Delete
        /// <summary>
        /// ResourceInterfaces cho Delete
        /// </summary>
        /// <param name="entityName"> Tên bảng/Tên đối tượng </param>
        /// <returns> Chuỗi resource </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string DeleteSuccess(string? entityName = null);
        string DeleteFailure(string? entityName = null);
        string DeleteMultiSuccess(string? entityName = null);
        string DeletedSomeItems(string? entityName = null);
        string DeleteMultiFailure(string? entityName = null);
        #endregion

        #region Block
        string BlockSuccess(string? objectName = null);
        string BlockFailure(string? objectName = null);
        #endregion

        #region Resource for Filter and GetNewCode

        /// <summary>
        /// ResourceInterfaces cho Filter
        /// </summary>
        /// <param name="entityName"> Tên bảng/Tên đối tượng </param>
        /// <returns> Chuỗi resource </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string FilterSuccess(string? entityName = null);
        string FilterFailure(string? entityName = null);


        /// <summary>
        /// ResourceInterfaces cho GetNewCode
        /// </summary>
        /// <param name="entityName"> Tên bảng/Tên đối tượng </param>
        /// <returns> Chuỗi resource </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string GetNewCodeSuccess(string? entityName = null);
        string GetNewCodeFailure(string? entityName = null);

        #endregion


        #region Response Messages
        /// <summary>
        /// Các ResourceInterfaces liên quan đến Message trả về
        /// </summary>
        /// <param name="entityName"> Tên bảng/Tên đối tượng </param>
        /// <returns> Chuỗi resource </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string NotFound(string? entityName = null);
        string NotExist(string? entityName = null);

        string ServerError(string? entityName = null);
        string UndefinedError(string? entityName = null);

        string EmptyId(string? entityName = null);
        string ExistCode(string? entityName = null);
        string ExistName(string? entityName = null);
        string EmptyFile(string? entityName = null);
        string CacheSave(string? entityName = null);
        string NotBeImplemented(); // "this api hasn't yet been Implemented"
        string Success();
        string Failure();
        #endregion


        #region Resource Transaction
        /// <summary>
        /// Response liên quan đến transaction
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        string TransactionNotOpen();
        string TransactionNotClose();

        #endregion


        #region Response for Authentication
        /// <summary>
        /// Resources cho response trả về của các service authentication
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (24/2/24)
        /// Modified: None
        string LoginSuccess();
        string LoginFailure();
        string InvalidUsername();
        string NotExistUser();
        string ExistedUser();
        string LogoutSuccess();
        string LogoutFailure();
        string InvalidToken();
        string RefreshTokenSuccess();
        string RefreshTokenFailure();
        string WrongOldPassword();
        string NewPasswordSameOldPassword();
        string ChangePasswordSuccess();
        string ChangePasswordFailure();
        string WaitInSecond(int seconds);
        string ForgotPasswordEmailSubject();
        string ForgotPasswordEmailContent(string code, int durationInMinutes = 3);
        string SendEmailSuccess();
        string InvalidVerifyCode();
        string InvalidAccessCode();
        string WrongVerifyCode(int remailAttemps);
        string VerifyCodeSuccess();
        string ResetPasswordSuccess();
        string RegistrationEmailSubject();
        string RegistrationEmailContent(string code, int durationInMinutes = 3);
        string AddNewUserSuccess();
        string RegisterAdminSuccess();
        string CaptchaCreated();
        string LimitLoginTime(); // "Đăng nhập quá số lần cho phép"
        string InvalidCaptcha();
        #endregion

    }
}
