using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UserProfileModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces.UserIteractions
{
    public interface IUserService
    {
        #region For User
        /// <summary>
        /// Lấy về chi tiết Profile của user khác
        /// </summary>
        /// <param name="myuid"> uid của user cần lấy </param>
        /// <param name="unOruid"> username hoặc userid của user cần lấy </param>
        /// <returns> Profile </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> GetUserDetail(Guid myuid, string unOruid);

        /// <summary>
        /// Tìm kiếm danh sách user theo từ khoa
        /// </summary>
        /// <param name="myuid"> id của bản thân </param>
        /// <param name="searchKey"> TỪ khóa tìm kiếm</param>
        /// Tiêu chí tìm kiếm: FullName, Username, Phonenumber nếu có
        /// <param name="page"> phan trang </param>
        /// <returns> Danh sách User tìm được </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> SearchUser(Guid? myuid, string searchKey, PaginationDto page);

        /// <summary>
        /// Lấy về chi tiết Profile của chính user
        /// </summary>
        /// <param name="myuid"> uid của user cần lấy </param>
        /// <returns> Profile </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> GetMyUserProfile(Guid myuid);

        /// <summary>
        /// Cập nhật thông tin profile của mình
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyUserProfile(Guid uid, UpdateProfileModel updateModel);

        /// <summary>
        /// Cập nhật ảnh đại điện
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyAvatarImage(Guid uid, UploadImageModel imageModel);

        /// <summary>
        /// Cập nhật ảnh bìa
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyCoverImage(Guid uid, UploadImageModel imageModel);

        /// <summary>
        /// Cập nhật ảnh dai dien
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyAvatarUrl(Guid uid, string? url);

        /// <summary>
        /// Cập nhật ảnh bìa
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyCoverUrl(Guid uid, string? url);

        /// <summary>
        /// Cập nhật fullname
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyFullname(Guid uid, string? fullname);

        /// <summary>
        /// Cập nhật avatar, anh bia va fullname
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyGeneralInfo(Guid uid, UpdateGeneralInforModel model);

        /// <summary>
        /// Cập nhật Bio
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (8/5/24)
        /// Modified: None
        Task<ServiceResult> UpdateMyBio(Guid uid, string? Bio);
        #endregion


        #region For Admin

        /// <summary>
        /// Admin lay ve danh sach user
        /// </summary>
        /// <param name="page"> phan trang </param>
        /// <returns> Danh sách Profile </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetListUser(PaginationDto page);

        /// <summary>
        /// Tìm kiếm danh sách user theo từ khóa bởi admin
        /// </summary>
        /// <param name="searchKey"> Từ khóa tìm kiếm </param>
        /// Tiêu chí tìm kiếm: FullName, Username, Phonenumber nếu có
        /// <param name="page"> phan trang </param>
        /// <returns> Danh sách Profile </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> AdminSearchUser(string searchKey, PaginationDto page);

        /// <summary>
        /// Admin Lấy về chi tiết Profile của user khác
        /// </summary>
        /// <param name="unOruid"> username hoặc userid của user cần lấy </param>
        /// <returns> Profile </returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> AdminGetUserProfile(string unOruid);

        /// <summary>
        /// Admin cập nhật thông tin tài khoản cho một user
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> AdminUpdateUserInfo(Guid uid, UpdateUserModel model);

        /// <summary>
        /// Admin khóa người dùng
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> AdminBlockUser(Guid uid);

        /// <summary>
        /// Admin mở khóa người dùng
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> AdminUnblockUser(Guid uid);

        /// <summary>
        /// Admin xóa một user
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/3/24)
        /// Modified: None
        Task<ServiceResult> AdminDeleteUser(Guid uid);
        #endregion
    }
}
