using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.VisualBasic;
using Mysqlx.Crud;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KnowledgeSharingApi.Services.Services
{
    public class NotificationService(
        IResourceFactory resourceFactory,
        INotificationRepository notificationRepository,
        IUserRepository userRepository
    ) : INotificationService
    {
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly INotificationRepository NotificationRepository = notificationRepository;
        protected readonly IUserRepository UserRepository = userRepository;

        protected readonly int LimitDefault = 20;
        protected readonly string NotificationResource = resourceFactory.GetEntityResource().Notification();


        #region Functional methods
        /// <summary>
        /// Kiểm tra user tồn tại và trả về ViewUser
        /// </summary>
        /// <param name="userId"> Id của người dùng cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<ViewUser> CheckUserExisted(Guid userId)
        {
            ViewUser viewUser = await UserRepository.GetDetail(userId)
                ?? throw new ValidatorException(ResponseResource.NotExist(ResourceFactory.GetEntityResource().User()));
            return viewUser;
        }

        /// <summary>
        /// Kiểm tra thông báo tồn tại và trả về Notification
        /// </summary>
        /// <param name="userId"> Id thông báo </param>
        /// <returns></returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        protected virtual async Task<Notification> CheckNotificationExisted(Guid notiId)
        {
            Notification notification = await NotificationRepository.Get(notiId)
                ?? throw new ValidatorException(ResponseResource.NotExist(NotificationResource));
            return notification;
        }
        #endregion

        #region Delete methods

        public virtual async Task<ServiceResult> DeleteNotification(Guid userId, Guid notiId)
        {
            // Kiểm tra người dùng tồn tại
            ViewUser user = await CheckUserExisted(userId);

            // Kiểm tra Noti tồn tại
            Notification notification = await CheckNotificationExisted(notiId);

            // Kiểm tra Noti đúng là của user
            if (notification.UserId != user.UserId)
                return ServiceResult.Forbidden("Đây không phải noti của bạn");

            // Thực hiện xóa
            int res = await NotificationRepository.Delete(notiId);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.DeleteFailure(NotificationResource));

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.DeleteSuccess(NotificationResource));
        }

        public virtual async Task<ServiceResult> DeleteNotifications(Guid userId)
        {
            ViewUser user = await CheckUserExisted(userId);
            int rows = await NotificationRepository.DeleteAllNotification(user.UserId);
            return ServiceResult.Success(ResponseResource.DeletedSomeItems(NotificationResource), string.Empty, rows);
        }

        public virtual async Task<ServiceResult> DeleteNotifications(Guid userId, Guid[] notiIds)
        {
            ViewUser user = await CheckUserExisted(userId);
            List<Notification?> notifications = await NotificationRepository.Get(notiIds);
            notifications = notifications.Where(noti => noti?.UserId == user.UserId).ToList();
            int rows = await NotificationRepository.Delete(
                notifications.Select(noti => noti?.NotificationId ?? Guid.Empty).ToArray()
            );
            return ServiceResult.Success(ResponseResource.DeletedSomeItems(NotificationResource), string.Empty, rows);
        }

        #endregion

        #region Get Methods

        public virtual async Task<ServiceResult> GetNotification(Guid userId, Guid notificationId)
        {
            ViewUser user = await CheckUserExisted(userId);
            Notification noti = await CheckNotificationExisted(notificationId);
            if (noti.UserId != user.UserId)
                return ServiceResult.Forbidden("Đây không phải là thông báo của bạn");
            return ServiceResult.Success(ResponseResource.GetSuccess(NotificationResource), string.Empty, noti);
        }

        public virtual async Task<ServiceResult> GetNotifications(Guid userId, PaginationDto pagination)
        {
            ViewUser user = await CheckUserExisted(userId);
            List<Notification> listNotification = 
                await NotificationRepository.GetNotificationsByUserId(user.UserId, pagination);
            return ServiceResult.Success(ResponseResource.GetMultiSuccess(NotificationResource), string.Empty, listNotification);
        }

        public virtual async Task<ServiceResult> GetNotifications(Guid userId, Guid[] notiIds)
        {
            ViewUser user = await CheckUserExisted(userId);
            List<Notification?> listNotification =
                (await NotificationRepository.Get(notiIds))
                .Where(noti => noti?.UserId == user.UserId)
                .ToList();
            return ServiceResult.Success(
                ResponseResource.GetMultiSuccess(NotificationResource), 
                string.Empty, 
                listNotification
            );
        }

        #endregion

        #region Set read methods

        public virtual async Task<ServiceResult> SetReadNotification(Guid userId, Guid notiId)
        {
            ViewUser user = await CheckUserExisted(userId);
            Notification noti = await CheckNotificationExisted(notiId);
            if (noti.UserId == user.UserId)
                return ServiceResult.Forbidden("Đây không phải thông báo của bạn");
            if (noti.IsRead)
                return ServiceResult.BadRequest("Bạn đã đọc thông báo này rồi");
            noti.IsRead = true;
            noti.ModifiedTime = DateTime.UtcNow;
            noti.ModifiedBy = user.Username;
            int res = await NotificationRepository.Update(notiId, noti);
            if (res <= 0) return ServiceResult.ServerError(ResponseResource.UpdateFailure());
            return ServiceResult.Success(ResponseResource.UpdateSuccess());
        }

        public virtual async Task<ServiceResult> SetReadNotifications(Guid userId)
        {
            ViewUser user = await CheckUserExisted(userId);
            int effects = await NotificationRepository.SetReadNotification(user.UserId);
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, effects);
        }

        public virtual async Task<ServiceResult> SetReadNotifications(Guid userId, Guid[] notiIds)
        {
            ViewUser user = await CheckUserExisted(userId);
            int effects = await NotificationRepository.SetReadNotification(user.UserId, notiIds);
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, effects);
        } 
        #endregion
    }
}
