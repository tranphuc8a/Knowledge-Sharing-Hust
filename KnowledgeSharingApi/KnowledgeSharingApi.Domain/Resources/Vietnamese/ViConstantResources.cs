using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Resources.Vietnamese
{
    public static class ViConstantResource
    {
        public const string CREATED_DATE_GREATER_THAN_TODAY = "Ngày tạo không được lớn hơn ngày hôm nay";
        public const string MODIFIED_DATE_GREATER_THAN_TODAY = "Ngày sửa không được lớn hơn ngày hôm nay";
        public const string DOB_GREATER_THAN_TODAY = "Ngày sinh không được lớn hơn ngày hôm nay";
        public const string IDENTIFY_DATE_GREATER_THAN_TODAY = "Ngày cấp căn cước không được lớn hơn ngày hôm nay";
        public const string JOIN_DATE_GREATER_THAN_TODAY = "Ngày tham gia không được lớn hơn ngày hôm nay";


        public const string PHONE_FORMAT = "Không đúng định dạng số điện thoại";
        public const string EMAIL_FORMAT = "Không đúng định dạng email";
        public const string DATETIME_FORMAT = "Không đúng định dạng ngày tháng";
        public const string IDENTITY_FORMAT = "Căn cước công dân phải là chuỗi số dài 9 hoặc 12 số";
        public const string SALARY_FORMAT = "Lương phải là số thực không âm";
        public const string CUSTOMERGROUP_NAME_FORMAT = "Tên nhóm khách hàng không hợp lệ (vip, normal)";
        public const string DEBT_FORMAT = "Số tiền nợ phải là số thực không âm";
        public const string BANK_ACCOUNT_FORMAT = "Số tài khoản phải là chuỗi số dài 4-20 ký tự";
        public const string GENDER_FORMAT = "Giới tính không hợp lệ (0, 1, 2)";
        public const string EXCEL_FORMAT = "Không đúng định dạng file excel";
        public const string USERNAME_FORMAT = "Không đúng định dạng username";
        public const string PASSWORD_FORMAT = "Không đúng định dạng password";


        public const string NAME_EMPTY = "Họ và tên không được trống";
        public const string SEARCH_KEY_EMPTY = "Từ khóa tìm kiếm không được trống";
        public const string CUSTOMERGROUP_NAME_EMPTY = "Tên nhóm khách hàng không được trống";
        public const string CUSTOMER_NAME_EMPTY = "Tên khách hàng không được trống";
        public const string EMPLOYEE_NAME_EMPTY = "Tên nhân viên không được trống";
        public const string DEPARTMENT_NAME_EMPTY = "Tên phòng ban không được trống";
        public const string POSITION_NAME_EMPTY = "Tên chức vụ không được trống";
        public const string EMAIL_EMPTY = "Email không được trống";
        public const string USERNAME_EMPTY = "Username không được trống";
        public const string PASSWORD_EMPTY = "Password không được trống";
        public const string USERID_EMPTY = "Mã người dùng (UserId) không được trống";
        public const string ROLEID_EMPTY = "Mã quyền (RoleId) không được trống";
        public const string ROLE_NAME_EMPTY = "Tên quyền (Rolename) không được trống";
        public const string ACCESS_TOKEN_EMPTY = "Access Token không được trống";
        public const string REFRESH_TOKEN_EMPTY = "Refresh Token không được trống";

        public const string EMPLOYEE_CODE_EMPTY = "Mã nhân viên không được trống";
        public const string CUSTOMER_CODE_EMPTY = "Mã khách hàng không được trống";
        public const string DEPARTMENT_CODE_EMPTY = "Mã phòng ban không được trống";
        public const string POSITION_CODE_EMPTY = "Mã chức vụ không được trống";
        public const string ACCESS_CODE_EMPTY = "Mã truy cập AccessCode không được trống";
        public const string VERIFY_CODE_EMPTY = "Mã xác minh Code không được trống";


        public const string CUSTOMER_CODE_LENGTH = "Độ dài mã khách hàng phải từ 6-20 ký tự";
        public const string POSITION_CODE_LENGTH = "Độ dài mã chức vụ phải từ 6-20 ký tự";
        public const string CUSTOMERGROUP_CODE_LENGTH = "Độ dài mã nhóm khách hàng phải từ 6-20 ký tự";
        public const string EMPLOYEE_CODE_LENGTH = "Độ dài mã nhân viên phải từ 6-20 ký tự";
        public const string DEPARTMENT_CODE_LENGTH = "Độ dài mã phòng ban phải từ 6-20 ký tự";


        public const string DUPLICATE_EMPLOYEE_CODE_EXCEL = "Mã nhân viên bị trùng với mã nhân viên khác trong tệp";
        public const string DUPLICATE_PHONE_NUMBER_EXCEL = "Số điện thoại bị trùng với số điện thoại khác trong tệp";
        public const string EXISTED_EMPLOYEE_CODE = "Mã nhân viên đã tồn tại trong hệ thống";
        public const string EXISTED_PHONE_NUMBER = "Số điện thoại đã tồn tại trong hệ thống";
        public const string NOT_EXISTED_KEY_CACHE = "Khóa cache đã hết hạn hoặc không tồn tại";

        public const string SERVER_ERROR = "Lỗi máy chủ, vui lòng liên hệ admin để được hỗ trợ.";
        public const string UNAUTHORIZED = "Bạn chưa đăng nhập. Vui lòng đăng nhập để thực hiện chức năng này.";
        public const string FORBIDDEN = "Bạn không có quyền truy cập tài nguyên này. Vui lòng liên hệ admin để được hỗ trợ";
    }
}
