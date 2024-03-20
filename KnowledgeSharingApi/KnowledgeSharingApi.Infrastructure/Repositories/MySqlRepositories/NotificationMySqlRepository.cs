using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class NotificationMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Notification>(dbContext), INotificationRepository
    {
        public async Task<int> DeleteAllNotification(string userId)
        {
            IEnumerable<Notification> notifications = 
                await DbContext.Notifications
                .Where(notification => notification.UserId.ToString() == userId)
                .ToListAsync();
            DbContext.Notifications.RemoveRange(notifications);
            return await DbContext.SaveChangeAsync();
        }

        public async Task<int> DeleteMultiNotificationByUserId(string userId, string[] ids)
        {
            IEnumerable<Notification> notifications =
                await DbContext.Notifications
                .Where(noti => noti.UserId.ToString() == userId && ids.Contains(noti.NotificationId.ToString()))
                .ToListAsync();
            DbContext.Notifications.RemoveRange(notifications);
            return await DbContext.SaveChangeAsync();
        }

        public Task<IEnumerable<Notification>> GetAllNotificationsByUserId(string userId)
        {
            IEnumerable<Notification> notifications =
                DbContext.Notifications
                .Where(noti => noti.UserId.ToString() == userId)
                .OrderByDescending(noti => noti.Time)
                .AsEnumerable();
            return Task.FromResult(notifications);
        }

        public Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId, int limit, int offset)
        {
            IEnumerable<Notification> notifications =
                DbContext.Notifications
                .Where(noti => noti.UserId.ToString() == userId)
                .OrderByDescending(noti => noti.Time)
                .Skip(offset).Take(limit)
                .AsEnumerable();
            return Task.FromResult(notifications);
        }

        public async Task<int> SetReadNotification(string userId)
        {
            List<Notification>
                notifications = await DbContext.Notifications
                .Where(noti => noti.UserId.ToString() == userId && !noti.IsRead)
                .ToListAsync();
            notifications.ForEach(noti => noti.IsRead = true);
            return await DbContext.SaveChangeAsync();
        }

        public async Task<int> SetReadNotification(string userId, string[] ids)
        {
            List<Notification> notifications = await DbContext.Notifications
                .Where(noti => noti.UserId.ToString() == userId && !noti.IsRead && ids.Contains(noti.NotificationId.ToString()))
                .ToListAsync();
            notifications.ForEach(noti => noti.IsRead = true);
            return await DbContext.SaveChangeAsync();
        }
    }
}
