using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IStarService
    {
        /// <summary>
        /// Anonymous lấy về danh sách star của một user item
        /// </summary>
        /// <param name="userItemId"> id của item cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetUserItemStars(Guid userItemId, int? limit, int? offset);

        /// <summary>
        /// User lấy về danh sách star của một user item
        /// </summary>
        /// <param name="userItemId"> id của item cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetUserItemStars(Guid myUid, Guid userItemId, int? limit, int? offset);

        /// <summary>
        /// User lấy về danh sách user item mà mình đã đánh giá số sao
        /// </summary>
        /// <param name="myUid"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyScoredUserItems(Guid myUid, int? limit, int? offset);
        Task<ServiceResult> UserGetMyScoredComments(Guid myUid, int? limit, int? offset);
        Task<ServiceResult> UserGetMyScoredCourses(Guid myUid, int? limit, int? offset);
        Task<ServiceResult> UserGetMyScoredQuestions(Guid myUid, int? limit, int? offset);
        Task<ServiceResult> UserGetMyScoredLessons(Guid myUid, int? limit, int? offset);
        Task<ServiceResult> UserGetMyScoredPosts(Guid myUid, int? limit, int? offset);

        /// <summary>
        /// User đánh giá star cho một item
        /// </summary>
        /// <param name="myUid"> id của user đánh giá </param>
        /// <param name="scoreModel"> Giá trị đánh giá </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> UserPutScores(Guid myUid, PutScoreModel scoreModel);

        /// <summary>
        /// Admin lấy về danh sách star của một user item
        /// </summary>
        /// <param name="userItemId"> id của item cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetUserItemStars(Guid userItemId, int? limit, int? offset);

    }
}
