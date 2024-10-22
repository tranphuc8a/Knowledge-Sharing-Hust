﻿using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        #region Course Register

        /// <summary>
        /// Lấy về danh sách user đã đăng ký tham gia khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<List<ViewCourseRegister>> GetRegistersOfCourse(Guid courseId);

        /// <summary>
        /// Lấy về danh sách user đã đăng ký tham gia khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<List<ViewCourseRegister>> GetRegistersOfCourse(Guid courseId, PaginationDto pagination);

        /// <summary>
        /// Lấy về danh sách khóa học đã đăng ký của một user
        /// </summary>
        /// <param name="userId"> id của user </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<List<ViewCourseRegister>> GetRegistersOfUser(Guid userId);

        /// <summary>
        /// Lấy về danh sách khóa học đã đăng ký của một user
        /// </summary>
        /// <param name="userId"> id của user </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<List<ViewCourseRegister>> GetRegistersOfUser(Guid userId, PaginationDto pagination);

        /// <summary>
        /// Kiểm tra một user có tham gia khóa học không
        /// CÓ: Trả về ViewCourseRegister tương ứng
        /// KHÔNG: Ném ra NotExistedEntityException
        /// </summary>
        /// <param name="userId"> id của user cần kiểm tra </param>
        /// <param name="courseId"> id của khóa học cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewCourseRegister?> GetViewCourseRegister(Guid userId, Guid courseId);
        #endregion



        #region Get View Courses

        /// <summary>
        /// Kiểm tra khóa học tồn tại và trả về chính khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần kiểm tra </param>
        /// <param name="errorMessage"> Cảnh báo lỗi </param>
        /// <returns> ViewCourse | throw NotExistedEntityException </returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<ViewCourse> CheckExistedCourse(Guid courseId, string errorMessage);

        /// <summary>
        /// Lấy về ViewCourse theo id
        /// </summary>
        /// <param name="courseId"> id của course cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<ViewCourse?> GetViewCourse(Guid courseId);

        /// <summary>
        /// Lấy về Danh sách ViewCourse theo danh sách id
        /// </summary>
        /// <param name="courseIds"> danh sách id của các khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse?>> GetViewCourse(Guid[] courseIds);

        /// <summary>
        /// Lấy về Danh sách view course
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourse(PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourse(Guid myUid, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course công khai
        /// Lay toan bo khong phan trang
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetPublicViewCourse();

        /// <summary>
        /// Lấy về Danh sách view course công khai
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetPublicViewCourse(PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course công khai của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetPublicViewCourseOfUser(Guid userId);

        /// <summary>
        /// Lấy về Danh sách view course của một user (cho admin)
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfUser(Guid userId);

        /// <summary>
        /// Lấy về Danh sách view course của một user cho một user khác
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfUser(Guid myUid, Guid userId);

        /// <summary>
        /// Lấy về Danh sách view course của một catefories
        /// </summary>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfCategory(string catName, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course public của một catefories
        /// </summary>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetPublicViewCourseOfCategory(string catName, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course của một catefories theo user
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetViewCourseOfCategory(Guid myUid, string catName, PaginationDto pagination);

        /// <summary>
        /// Lấy về Danh sách view course của một user đã mark
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<ViewCourse>> GetMarkedCoursesOfUser(Guid userId, PaginationDto pagination);

        #endregion


        #region Get T


        /// <summary>
        /// Lấy về ViewCourse theo id
        /// </summary>
        /// <param name="courseId"> id của course cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<T?> GetViewCourse<T>(Guid courseId, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách ViewCourse theo danh sách id
        /// </summary>
        /// <param name="courseIds"> danh sách id của các khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T?>> GetViewCourse<T>(Guid[] courseIds, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetViewCourse<T>(PaginationDto pagination, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetViewCourse<T>(Guid myUid, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course công khai
        /// Lay toan bo khong phan trang
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetPublicViewCourse<T>(Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course công khai
        /// </summary>
        /// <param name="pagination"> Thuoc tinh phan trang </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetPublicViewCourse<T>(PaginationDto pagination, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course công khai của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetPublicViewCourseOfUser<T>(Guid userId, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course của một user (cho admin)
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetViewCourseOfUser<T>(Guid userId, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course của một user cho một user khác
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetViewCourseOfUser<T>(Guid myUid, Guid userId, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course của một catefories
        /// </summary>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetViewCourseOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course public của một catefories
        /// </summary>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetPublicViewCourseOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course của một catefories theo user
        /// </summary>
        /// <param name="myUid"> id của user muốn lấy </param>
        /// <param name="catName"> tên category muốn lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetViewCourseOfCategory<T>(Guid myUid, string catName, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector);

        /// <summary>
        /// Lấy về Danh sách view course của một user đã mark
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (30/03/24)
        /// Modified: None
        Task<List<T>> GetMarkedCoursesOfUser<T>(Guid userId, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector);


        #endregion

        /// <summary>
        /// Lay ve quan he cua user voi mot danh sach khoa hoc
        /// </summary>
        /// <param name="userId"> id cua user thuc hien </param>
        /// <param name="courseIds"> Danh sach course id </param>
        /// <returns></returns>
        /// Created: PhucTV (13/5/24)
        /// Modified: None
        Task<Dictionary<Guid, CourseRoleTypeDto>> GetCourseRoleType(Guid userId, List<Guid> courseIds);
    }
}
