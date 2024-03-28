using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ILessonService : IBasePostService
    {

        /// <summary>
        /// User thay đổi quyền truy cập của bài học
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="model"> Thông tin thay đổi privacy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> ChangePrivacy(Guid myUid, ChangeKnowledgePrivacyModel model);


        /// <summary>
        /// Lấy về danh sách khóa học có sử dụng bài giảng hiện tại
        /// </summary>
        /// <param name="myUId"> id của người cần lấy </param>
        /// <param name="lessonId"> id của bài giảng cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetListCourseOfLesson(Guid myUId, Guid lessonId, int? limit, int? offset);
    }

}
