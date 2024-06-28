using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;

namespace KnowledgeSharingApi.Repositories.Interfaces.Repositories
{
    public interface IBaseUserItemRepository
    {
        /// <summary>
        /// Hai hàm lấy về chính xác kiểu và dữ liệu của userItem
        /// </summary>
        /// <param name="userItemId"> id của item cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<IViewUserItem?> GetExactlyUserItem(Guid userItemId);
        Task<Dictionary<Guid, IViewUserItem?>> GetExactlyUserItem(List<Guid> userItemId);
        Task<IResponseUserItemModel?> GetExactlyResponseUserItemModel(Guid userItemId);
        Task<Dictionary<Guid, IResponseUserItemModel?>> GetExactlyResponseUserItemModel(List<Guid> userIds);

        /// <summary>
        /// Ham devide mot list useritem ra thanh cac list cu the hon
        /// </summary>
        /// <param name="list"> Danh sach can devide </param>
        /// <param name="comments"> output: Danh sach comment </param>
        /// <param name="courses"> output: Danh sach course </param>
        /// <param name="lessons"> output: Danh sach lesson </param>
        /// <param name="questions"> output: Danh sach question </param>
        /// CReated: PhucTV (10/5/24)
        /// Modified: None
        void DevideUserItem(
            List<UserItem> list,
            out List<Comment> comments,
            out List<Course> courses,
            out List<Lesson> lessons,
            out List<Question> questions);
        void DevideResponseUserItemModel(
            List<IResponseUserItemModel> list,
            List<ResponseCommentModel> comments,
            List<ResponseCourseModel> courses,
            List<ResponseLessonModel> lessons,
            List<ResponseQuestionModel> questions
            );
    }
}
