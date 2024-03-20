using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Resources.Vietnamese
{
    public class ViEntityResource : IEntityResource
    {
        public string Abstract(string? entityName)
        {
            return entityName == null ?
                $"Tóm tắt": $"Tóm tắt {entityName}";
        }

        public string Avatar()
        {
            return $"Ảnh đại điện";
        }

        public string Block()
        {
            return $"Chặn";
        }

        public string Category()
        {
            return $"Thể loại";
        }

        public string Comment()
        {
            return $"Bình luận";
        }

        public string Conversation()
        {
            return $"Cuộc trò chuyện";
        }

        public string Content(string? entityName)
        {
            return entityName == null ? $"Nội dung" : $"Nội dung của ${entityName}";
        }

        public string Course()
        {
            return $"Khóa học";
        }

        public string CourseLesson()
        {
            return $"Bài học trong khóa học";
        }

        public string CoursePayment()
        {
            return $"Thanh toán của khóa học";
        }

        public string CourseRegister()
        {
            return $"Đăng ký khóa học";
        }

        public string CourseRelation()
        {
            return $"Yêu cầu/Lời mời tham gia khóa học";
        }

        public string Cover()
        {
            return $"Ảnh bìa";
        }

        public string DateOfBirth()
        {
            return $"Ngày sinh";
        }

        public string Email()
        {
            return $"Địa chỉ thư điện tử";
        }

        public string Follow()
        {
            return $"Theo dõi";
        }

        public string Friend()
        {
            return $"Bạn bè";
        }

        public string Id(string? entityName)
        {
            return entityName == null ? $"Id" : $"Id của {entityName}";
        }

        public string Introduction(string? entityName)
        {
            return entityName == null ? $"Giới thiệu" : $"Giới thiệu của {entityName}";
        }

        public string Knowledge()
        {
            return $"Phần tử kiến thức";
        }

        public string KnowledgeCategory()
        {
            return $"Thể loại của phần tử kiến thức";
        }

        public string Lesson()
        {
            return $"Bài học/Bài giảng";
        }

        public string Mark()
        {
            return $"Đánh dấu/Lưu";
        }

        public string Message()
        {
            return $"Tin nhắn";
        }

        public string Name(string? entityName)
        {
            return entityName == null ? $"Tên" : $"Tên {entityName}";
        }

        public string Notification()
        {
            return $"Thông báo";
        }

        public string Password()
        {
            return $"Mật khẩu";
        }

        public string Post()
        {
            return $"Bài đăng";
        }

        public string PostEditHistory()
        {
            return $"Lịch sử chỉnh sửa bài đăng";
        }

        public string Privacy(string? entityName)
        {
            return entityName == null ? $"Quyền riêng tư" : $"Quyền riêng tư của {entityName}";
        }

        public string Profile()
        {
            return $"Thông tin cá nhân";
        }

        public string Question()
        {
            return $"Câu hỏi/Thảo luận";
        }

        public string Receicer()
        {
            return $"Người nhận";
        }

        public string RequestFriend()
        {
            return $"Yêu cầu kết bạn";
        }

        public string Sender()
        {
            return $"Người gửi";
        }

        public string Session()
        {
            return $"Phiên đăng nhập";
        }

        public string Star()
        {
            return $"Điểm/Sao";
        }

        public string StudyProgress()
        {
            return $"Tiến trình học tập";
        }

        public string Thumbnail(string? entityName)
        {
            return $"Url ảnh thumbnail";
        }

        public string Time()
        {
            return $"Thời gian";
        }

        public string Title(string? entityName)
        {
            return $"Tiêu đề{entityName ?? $" cúa {entityName}"}";
        }

        public string User()
        {
            return $"Người dùng";
        }

        public string UserConversation()
        {
            return $"Người dùng trong cuộc trò chuyện";
        }

        public string UserItem()
        {
            return $"Phần tử thông tin người dùng";
        }

        public string Username()
        {
            return $"Tên người dùng";
        }

        public string UserRelation()
        {
            return $"Quan hệ người dùng";
        }
    }
}
