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
        public async Task<int> DeleteAllNotification(Guid userId)
        {
            IEnumerable<Notification> notifications = 
                await DbContext.Notifications
                .Where(notification => notification.UserId == userId)
                .ToListAsync();
            DbContext.Notifications.RemoveRange(notifications);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteMultiNotificationByUserId(Guid userId, Guid[] ids)
        {
            IEnumerable<Notification> notifications =
                await DbContext.Notifications
                .Where(noti => noti.UserId == userId && ids.Contains(noti.NotificationId))
                .ToListAsync();
            DbContext.Notifications.RemoveRange(notifications);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsByUserId(Guid userId)
        {
            var notifications =
                DbContext.Notifications
                .Where(noti => noti.UserId == userId)
                .OrderByDescending(noti => noti.Time);
            return await notifications.ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserId(Guid userId, int limit, int offset)
        {
            var notifications =
                DbContext.Notifications
                .Where(noti => noti.UserId == userId)
                .OrderByDescending(noti => noti.Time)
                .Skip(offset).Take(limit);
            return await notifications.ToListAsync();
        }

        public async Task<int> SetReadNotification(Guid userId)
        {
            List<Notification>
                notifications = await DbContext.Notifications
                .Where(noti => noti.UserId == userId && !noti.IsRead)
                .ToListAsync();
            notifications.ForEach(noti => noti.IsRead = true);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> SetReadNotification(Guid userId, Guid[] ids)
        {
            List<Notification> notifications = await DbContext.Notifications
                .Where(noti => noti.UserId == userId && !noti.IsRead && ids.Contains(noti.NotificationId))
                .ToListAsync();
            notifications.ForEach(noti => noti.IsRead = true);
            return await DbContext.SaveChangesAsync();
        }
    }
}
