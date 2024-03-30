using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface ICourseService
    {

        #region Categories and Mark Apiés
        /// <summary>
        /// Vấy về danh sách khóa học gắn với thẻ catName --> Nhóm API Category
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
        /// User lấy về danh sách khóa học mà mình đã đánh dấu --> Nhóm API Mark
        /// </summary>
        /// <param name="myUid"> id của người lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMarkedCourses(Guid myUid, int? limit, int? offset);
        #endregion


        #region Anonymous Apies

        /// <summary>
        /// Yêu cầu anonymous lấy về danh sách khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: Nones
        Task<ServiceResult> AnonymousGetCourses(int? limit, int? offset);

        /// <summary>
        /// Yêu cầu anonymous lấy về danh sách khóa học của một người dùng
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetUserCourses(Guid userId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu anonymous lấy về chi tiết khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ServiceResult> AnonymousGetCourseDetail(Guid courseId);

        #endregion


        #region Admin Apies

        /// <summary>
        /// Yêu cầu admin lấy về danh sách khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetCourses(int? limit, int? offset);


        /// <summary>
        /// Yêu cầu admin lấy về danh sách khóa học của một user
        /// </summary>
        /// <param name="userId"> id của người cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetUserCourses(Guid userId, int? limit, int? offset);


        /// <summary>
        /// Yêu cầu admin lấy về chi tiết khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetCourseDetail(Guid courseId);

        /// <summary>
        /// Yêu cầu admin lấy về danh sách khóa học đã mà một user đã đăng ký
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetUserRegisteredCourses(Guid userId, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu admin xóa một khóa học cụ thể
        /// </summary>
        /// <param name="courseId"> id của khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> AdminDeleteCourse(Guid courseId);


        #endregion


        #region User get apies

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetListCourses(Guid myUid, int? limit, int? offset);

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học của user khác
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetUserCourses(Guid myUid, Guid userId, int? limit, int?  offset);

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học của chính mình
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyCourses(Guid myUid, int? limit, int? offset);


        /// <summary>
        /// Yêu cầu user get về chi tiết một khóa học
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetCourseDetail(Guid myUid, Guid courseId);


        /// <summary>
        /// Yêu cầu user get về chi tiết khóa học của mình
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyCourseDetail(Guid myUid, Guid courseId);

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học mà mình đã đăng ký
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserGetMyRegisteredCourses(Guid myUid, int? limit, int? offset);


        #endregion


        #region user operations with course

        /// <summary>
        /// Yêu cầu user tạo mới một khóa học
        /// </summary>
        /// <param name="model"> thông tin khóa học mới được tạo ra </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserCreateCourse(Guid myUid, CreateCourseModel model);


        /// <summary>
        /// Yêu cầu user cập nhật thông tin của một khóa học
        /// </summary>
        /// <param name="myUid"> id của người dùng muốn thực hiện </param>
        /// <param name="courseId"> id của khóa học được cập nhật </param>
        /// <param name="model"> thông tin khóa học mới được cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserUpdateCourse(Guid myUid, Guid courseId, UpdateCourseModel model);


        /// <summary>
        /// Yêu cầu user xóa khóa học
        /// </summary>
        /// <param name="myUid"> Id user cần thực hiện </param>
        /// <param name="courseId"> Id của khóa học cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> UserDeleteCourse(Guid myUid, Guid courseId);


        /// <summary>
        /// Yêu cầu user cập nhật quyền riêng tư của khóa học
        /// </summary>
        /// <param name="myUid"> id của user cần thực hiện </param>
        /// <param name="model"> thông tin quyền riêng tư mới cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        Task<ServiceResult> ChangePrivacy(Guid myUid, ChangeKnowledgePrivacyModel model);


        #endregion

    }
}
