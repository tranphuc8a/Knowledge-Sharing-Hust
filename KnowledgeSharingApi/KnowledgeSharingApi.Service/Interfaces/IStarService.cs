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
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetUserItemStars(Guid userItemId, PaginationDto page);

        /// <summary>
        /// User lấy về danh sách star của một user item
        /// </summary>
        /// <param name="userItemId"> id của item cần lấy </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetUserItemStars(Guid myUid, Guid userItemId, PaginationDto page);

        /// <summary>
        /// User lấy về danh sách user item mà mình đã đánh giá số sao
        /// </summary>
        /// <param name="myUid"> id của user cần lấy </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyScoredUserItems(Guid myUid, PaginationDto page);
        Task<ServiceResult> UserGetMyScoredComments(Guid myUid, PaginationDto page);
        Task<ServiceResult> UserGetMyScoredCourses(Guid myUid, PaginationDto page);
        Task<ServiceResult> UserGetMyScoredQuestions(Guid myUid, PaginationDto page);
        Task<ServiceResult> UserGetMyScoredLessons(Guid myUid, PaginationDto page);
        Task<ServiceResult> UserGetMyScoredPosts(Guid myUid, PaginationDto page);

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
        /// User đánh giá star cho một item
        /// </summary>
        /// <param name="myUid"> id của user đánh giá </param>
        /// <param name="userItemId"> id item can xoa </param>
        /// <returns></returns>
        /// Created: PhucTV (30/5/24)
        /// Modified: None
        Task<ServiceResult> UserDeleteScores(Guid myUid, Guid userItemId);

        /// <summary>
        /// Admin lấy về danh sách star của một user item
        /// </summary>
        /// <param name="userItemId"> id của item cần lấy </param>
        /// <param name="page"> phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetUserItemStars(Guid userItemId, PaginationDto page);

    }
}
