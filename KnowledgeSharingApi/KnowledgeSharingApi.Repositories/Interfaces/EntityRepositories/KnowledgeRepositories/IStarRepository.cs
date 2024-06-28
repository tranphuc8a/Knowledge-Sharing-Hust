using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories
{
    public interface IStarRepository : IRepository<Star>
    {
        /// <summary>
        /// Thực hiện lấy về danh sách số sao mà user đánh giá cho một danh sách userItem
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userItemsId"> id của useritem cần lấy </param>
        /// <returns> null nếu chưa đánh giá </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<Dictionary<Guid, int?>> CalculateUserStars(Guid userId, List<Guid> userItemsId);

        /// <summary>
        /// Thực hiện lấy về trung bình số sao mà userItem đã được nhận
        /// </summary>
        /// <param name="userItemsId"> id của useritem cần lấy </param>
        /// <returns> null nếu chưa có ai đánh giá </returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<Dictionary<Guid, double?>> CalculateAverageStars(List<Guid> userItemsId);

        /// <summary>
        /// Thực hiện lấy về tổng số lượt đã đánh giá cho user Item
        /// </summary>
        /// <param name="userItemsId"> id của useritem cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<Dictionary<Guid, int>> CalculateTotalStars(List<Guid> userItemsId);


        /// <summary>
        /// Lấy về danh sách star của một user item
        /// </summary>
        /// <param name="userItemId"> id của user item cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<List<Star>> GetListStarOfUserItem(Guid userItemId);


        /// <summary>
        /// Lấy về danh sách các user item đã được đánh giá star của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<List<Star>> GetListStarOfUser(Guid userId);


        /// <summary>
        /// Lấy về star của user đánh giá một item
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="userId"> id của item cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (27/3/24)
        /// Modified: None
        Task<Star?> GetStarOfUserAndUserItem(Guid userId, Guid itemId);


        /// <summary>
        /// Lấy về danh sách các phần tử đã được đánh giá bởi userId
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns> List of tuple: Item1: phần tử, Item2: star </returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<List<Tuple<ViewComment, Star>>> GetStaredComments(Guid userId);
        Task<List<Tuple<ViewCourse, Star>>> GetStaredCourses(Guid userId);
        Task<List<Tuple<ViewPost, Star>>> GetStaredPosts(Guid userId);
        Task<List<Tuple<ViewQuestion, Star>>> GetStaredQuestions(Guid userId);
        Task<List<Tuple<ViewLesson, Star>>> GetStaredLessons(Guid userId);
    }
}
