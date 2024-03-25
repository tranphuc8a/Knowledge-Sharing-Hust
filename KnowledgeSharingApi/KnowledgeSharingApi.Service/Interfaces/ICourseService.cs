using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICourseService
    {

        /// <summary>
        /// Vấy về danh sách khóa học gắn với thẻ catName
        /// </summary>
        /// <param name="catName"> tên thẻ </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetListCourseOfCategory(string catName, int? limit, int? offset);
        Task<ServiceResult> UserGetListCourseOfCategory(Guid myUid, string catName, int? limit, int? offset);
        Task<ServiceResult> AdminListCourseOfCategory(string catName, int? limit, int? offset);

        /// <summary>
        /// User lấy về danh sách khóa hojc mà mình đã đánh dấu
        /// </summary>
        /// <param name="myUid"> id của người lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMarkedCourses(Guid myUid, int? limit, int? offset);
    }
}
