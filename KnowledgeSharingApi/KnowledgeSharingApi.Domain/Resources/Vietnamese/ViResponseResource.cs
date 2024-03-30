using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Resources.Vietnamese
{
    public class ViResponseResource : IResponseResource
    {
        public string DeletedSomeItems(string? entityName = null)
        {
            return entityName == null ?
                $"Đã xóa được một vài items" :
                $"Đã xóa được một vài {entityName}";
        }

        public string DeleteFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Xóa thất bại" :
                $"Xóa {entityName} thất bại";
        }

        public string DeleteMultiFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Không xóa được mục nào" :
                $"Không xóa được {entityName} nào";
        }

        public string DeleteMultiSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Xóa danh sách thành công" :
                $"Xóa danh sách {entityName} thành công";
        }

        public string DeleteSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Xóa thành công" :
                $"Xóa {entityName} thành công";
        }

        public string FilterFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Lọc thất bại" :
                $"Lọc thất bại danh sách {entityName}";
        }

        public string FilterSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Lọc thành công" :
                $"Lọc thành công danh sách {entityName}";
        }

        public string GetFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Lấy item thất bại" :
                $"Lấy {entityName} thất bại";
        }

        public string GetMultiFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Lấy danh sách thất bại" :
                $"Lấy danh sách {entityName} thất bại";
        }

        public string GetMultiSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Lấy danh sách thành công" :
                $"Lấy danh sách {entityName} thành công";
        }

        public string GetNewCodeFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Lấy mã mới thất bại" :
                $"Lấy mã {entityName} mới thất bại";
        }

        public string GetNewCodeSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Lấy mã mới thành công" :
                $"Lấy mã {entityName} mới thành công";
        }

        public string GetSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Lấy thông tin thành công" :
                $"Lấy thông tin {entityName} thành công";
        }

        public string InsertFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Thêm thất bại" :
                $"Thêm {entityName} thất bại";
        }

        public string InsertMultiFalure(string? entityName = null)
        {
            return entityName == null ?
                $"Thêm danh sách thất bại" :
                $"Thêm danh sách {entityName} thất bại";
        }

        public string InsertMultiSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Thêm danh sách thành công" :
                $"Thêm danh sách {entityName} thành công";
        }

        public string InsertSuccess(string? entityName = null)
        {
            if (entityName == null)
            {
                return "Thêm thành công";
            }
            return $"Thêm {entityName} thành công";
        }

        public string InsertedSomeItems(string? entityName = null)
        {
            if (entityName == null)
            {
                return "Thêm thành công một vài bản ghi";
            }
            return $"Đã thêm thành công một vài";
        }

        public string NotExist(string? entityName = null)
        {
            return entityName == null ?
                $"Không tồn tại" :
                $"{entityName} Không tồn tại";
        }

        public string NotFound(string? entityName = null)
        {
            return entityName == null ?
                $"Không tìm thấy" :
                $"Không tìm thấy {entityName}";
        }

        public string ServerError(string? entityName = null)
        {
            return entityName == null ?
                $"Lỗi hệ thống đã xảy ra" :
                $"Lỗi hệ thống đã xảy ra với {entityName}";
        }

        public string UndefinedError(string? entityName = null)
        {
            return entityName == null ?
                $"Lỗi không xác định đã xảy ra" :
                $"Lỗi không xác định đã xảy ra với {entityName}";
        }

        public string UpdateFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Cập nhật thất bại" :
                $"Cập nhật {entityName} thất bại";
        }

        public string UpdateMultiFailure(string? entityName = null)
        {
            return entityName == null ?
                $"Cập nhật danh sách thất bại" :
                $"Cập nhật danh sách {entityName} thất bại";
        }

        public string UpdateMultiSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Cập nhật danh sách thành công" :
                $"Cập nhật danh sách {entityName} thành công";
        }

        public string UpdateSuccess(string? entityName = null)
        {
            return entityName == null ?
                $"Cập nhật thành công" :
                $"Cập nhật {entityName} thành công";
        }

        public string EmptyId(string? entityName = null)
        {
            return entityName == null ?
                $"Trường Id không được trống" :
                $"Trường Id của {entityName} không được trống";
        }

        public string ExistName(string? entityName = null)
        {
            return entityName == null ?
                $"Tên đã tồn tại" :
                $"Tên {entityName} đã tồn tại";
        }
        public string ExistCode(string? entityName = null)
        {
            return entityName == null ?
                $"Mã đã tồn tại" :
                $"Mã {entityName} đã tồn tại";
        }

        public string EmptyFile(string? entityName = null)
        {
            return entityName == null ?
                $"File Import rỗng" :
                $"File {entityName} rỗng";
        }

        public string CacheSave(string? entityName = null)
        {
            return entityName == null ?
                $"Đã lưu dữ liệu ra cache" :
                $"Đã lưu dữ liệu {entityName} ra cache";
        }

        public string TransactionNotOpen()
        {
            return "Giao dịch bị null hoặc chưa được mở";
        }
        public string TransactionNotClose()
        {
            return "Giao dịch chưa kết thúc hoặc chưa đóng";
        }

        public string LoginSuccess()
        {
            return "Đăng nhập thành công";
        }

        public string LoginFailure()
        {
            return "Tên tài khoản hoặc mật khẩu không đúng";
        }

        public string InvalidUsername()
        {
            return "Tên tài khoản không hợp lệ";
        }

        public string NotExistUser()
        {
            return "Người dùng không tồn tại";
        }

        public string ExistedUser()
        {
            return "Người dùng đã tồn tại trong hệ thống";
        }

        public string LogoutSuccess()
        {
            return "Đăng xuất thành công";
        }

        public string LogoutFailure()
        {
            return "Đăng xuất thất bại";
        }

        public string InvalidToken()
        {
            return "Token đã hết hạn hoặc không hợp lệ";
        }

        public string RefreshTokenSuccess()
        {
            return "Làm mới token thành công";
        }

        public string RefreshTokenFailure()
        {
            return "Làm mới token thất bại";
        }

        public string WrongOldPassword()
        {
            return "Mật khẩu cũ không đúng";
        }

        public string NewPasswordSameOldPassword()
        {
            return "Mật khẩu mới không được trùng mật khẩu cũ";
        }

        public string ChangePasswordSuccess()
        {
            return "Đổi mật khẩu thành công";
        }

        public string ChangePasswordFailure()
        {
            return "Đổi mật khẩu thất bại";
        }

        public string WaitInSecond(int seconds)
        {
            return $"Cần phải đợi trong {seconds} giây nữa";
        }

        public string ForgotPasswordEmailSubject()
        {
            return "Mã lấy lại mật khẩu của bạn";
        }

        public string ForgotPasswordEmailContent(string code, int durationInMinutes = 3)
        {
            return $"Bạn vừa gửi yêu cầu đặt lại mật khẩu tài khoản của bạn trên Knowledge Sharing. <br/>" +
                    $"Nhập mã <h1>{code}</h1> để đặt lại mật khẩu. <br/> " +
                    $"Lưu ý không để lộ mã này cho người khác biết. <br/>" +
                    $"Mã có thời hạn trong {durationInMinutes} phút.";
        }

        public string SendEmailSuccess()
        {
            return "Đã gửi mã xác minh tới email của bạn";
        }

        public string InvalidVerifyCode()
        {
            return "Mã xác minh đã hết hạn hoặc không hợp lệ";
        }

        public string InvalidAccessCode()
        {
            return "Mã truy nhập AccessCode không hợp lệ";
        }

        public string WrongVerifyCode(int remainAttemps)
        {
            return $"Mã xác minh không hợp lệ. Bạn còn lại {remainAttemps} lần thử.";
        }

        public string VerifyCodeSuccess()
        {
            return "Mã xác minh hợp lệ";
        }

        public string ResetPasswordSuccess()
        {
            return "Đặt lại mật khẩu thành công";
        }

        public string ResetPasswordFailure()
        {
            return "Đặt lại mật khẩu thất bại";
        }

        public string RegistrationEmailSubject()
        {
            return "Mã xác minh đăng ký tài khoản Knowledge Sharing";
        }

        public string RegistrationEmailContent(string code, int durationInMinutes = 3)
        {
            return $"Bạn vừa gửi yêu cầu đăng ký tài khoản mới trên Knowledge Sharing. <br/> " +
                    $"Nhập mã <h1>{code}</h1> để đăng ký tài khoản. <br/> " +
                    $"Lưu ý không để lộ mã này cho người khác biết. <br/>" +
                    $"Mã có thời hạn trong {durationInMinutes} phút.";
        }

        public string AddNewUserSuccess()
        {
            return "Đăng ký tài khoản mới thành công";
        }        
        
        public string AddNewUserFailure()
        {
            return "Đăng ký tài khoản mới thất bại";
        }

        public string RegisterAdminSuccess()
        {
            return "Đăng ký Quản trị viên thành công";
        }

        public string BlockSuccess(string? objectName = null)
        {
            return objectName == null ? $"Đã chặn thành công" : $"Đã chặn {objectName} thành công";
        }

        public string BlockFailure(string? objectName = null)
        {
            return objectName == null ? $"Chặn thất bại" : $"Chặn {objectName} thất bại";
        }

        public string NotBeImplemented()
        {
            return $"API này vẫn chưa được hỗ trợ cài đặt";
        }

        public string Success()
        {
            return $"Thành công";
        }

        public string Failure()
        {
            return $"Thất bại";
        }

        public string CaptchaCreated()
        {
            return $"Captcha đã được tạo";
        }

        public string LimitLoginTime()
        {
            return $"Đăng nhập quá nhiều lần, vui lòng nhập mã captcha để tiếp tục";
        }

        public string InvalidCaptcha()
        {
            return $"Mã captcha không hợp lệ";
        }

        public string CancelUserEmailSubject()
        {
            return $"Mã xác minh HỦY tài khoản Knowledge Sharing";
        }

        public string CancelUserEmailContent(string code, int durationInMinutes = 3)
        {
            return $"Bạn vừa gửi yêu cầu hủy bỏ tài khoản của bạn trên Knowledge Sharing. <br/> " +
                    $"Nhập mã <h1>{code}</h1> để xác nhận hủy bỏ tài khoản. <br/> " +
                    $"Lưu ý không để lộ mã này cho người khác biết. <br/>" +
                    $"Mã có thời hạn trong {durationInMinutes} phút.";
        }

        public string CancelUserSuccess()
        {
            return $"Yêu cầu của bạn đã được đưa vào tiến trình xử lý. Chúng tôi sẽ gửi email tới bạn ngay khi tiến trình hoàn tất.";
        }

        public string CancelUserFailure()
        {
            return $"Hủy bỏ tài khoản thất bại";
        }

        public string CancelUserRespomseEmailSubject()
        {
            return $"Kết quả xử lý yêu cầu hủy bỏ tài khoản của bạn trên Knowledge Sharing";
        }

        public string CancelUserFailureProcessResponseEmailContent(string name)
        {
            return 
                $"Xin chào <b>{name}</b>, <br><br> " +
                $"Bạn đã gửi cho chúng tôi yêu cầu hủy tài khoản được đăng ký bởi email này của bạn trên <b>Knowledge Sharing</b>. <br> " +
                $"Chúng tôi rất tiếc phải thông báo rằng tiến trình đã gặp sự cố vì một số lý do. <br> " +
                $"Vui lòng thực hiện lại yêu cầu hoặc báo lại với chúng tôi bằng cách phản hồi lại email " +
                $"này để chúng tôi có thể trợ giúp bạn trong thời gian sớm nhất. <br><br> " +
                $"Rất xin lỗi bạn vì sự bất tiện này,<br> " +
                $"Đội ngũ <b>Knowledge Sharing</b> ";
        }

        public string CancelUserSuccessProcessResponseEmailContent(string name)
        {
            return
                $"Xin chào <b>{name}</b>, <br><br> " +
                $"Bạn đã gửi cho chúng tôi yêu cầu hủy tài khoản được đăng ký bởi email này của bạn trên <b>Knowledge Sharing</b>. <br> " +
                $"Yêu cầu của bạn đã được xử lý thành công: <i>Tài khoản của bạn đã bị xóa vĩnh viễn và không thể khôi phục</i>. <br> " +
                $"Giờ đây, bạn có thể dùng email này để đăng ký tài khoản mới trên <b>Knowledge Sharing</b>. <br><br> " +
                $"Hi vọng bạn đã có những trải nghiệm thú vị,<br> " +
                $"Cảm ơn bạn đã tham gia,<br> " +
                $"Đội ngũ <b>Knowledge Sharing</b> ";
        }

        public string CoursePaymentEmailSubject()
        {
            return $"Mã xác minh thanh toán khóa học Knowledge Sharing";
        }

        public string CoursePaymentEmailContent(string code, int durationInMinutes = 3)
        {
            return $"Bạn vừa gửi yêu cầu thanh toán một khóa học trên Knowledge Sharing. <br/> " +
                    $"Nhập mã <h1>{code}</h1> để xác minh thanh toán. <br/> " +
                    $"Lưu ý không để lộ mã này cho người khác biết. <br/>" +
                    $"Mã có thời hạn trong {durationInMinutes} phút.";
        }
    }
}
