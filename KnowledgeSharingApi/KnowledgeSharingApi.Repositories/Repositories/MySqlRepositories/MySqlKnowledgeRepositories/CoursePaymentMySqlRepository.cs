using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class CoursePaymentMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CoursePayment>(dbContext), ICoursePaymentRepository
    {
        public async Task<List<ViewCoursePayment>> GetByCourse(Guid courseId)
        {
            return await DbContext.ViewCoursePayments
                .Where(cp => cp.CourseId == courseId)
                .OrderByDescending(cp => cp.CreatedTime)
                .ToListAsync();
        }

        public async Task<List<ViewCoursePayment>> GetByUser(Guid userId)
        {
            return await DbContext.ViewCoursePayments
                .Where(cp => cp.UserId == userId)
                .OrderByDescending(cp => cp.CreatedTime)
                .ToListAsync();
        }

        public async Task<ViewCoursePayment?> GetCoursePayment(Guid paymentId)
        {
            return await DbContext.ViewCoursePayments.FindAsync(paymentId);
        }

        public async Task<List<ViewCoursePayment>> GetCoursePayment(Guid userId, Guid courseId)
        {
            return await DbContext.ViewCoursePayments
                .Where(cp => cp.UserId == userId && cp.CourseId == courseId)
                .OrderByDescending(cp => cp.CreatedTime)
                .ToListAsync();
        }

        /// <summary>
        /// Tao mot participant cua user tham gia khoa hoc
        /// </summary>
        /// <param name="user"> user tham gia khoa hoc </param>
        /// <param name="course"> khoa hoc </param>
        /// <returns></returns>
        /// Created: PhucTV (31/3/24)
        /// Modified: None
        protected virtual CourseRegister CreateCourseRegister(ViewUser user, ViewCourse course)
        {
            return new CourseRegister()
            {
                // Entity:
                CreatedBy = user.FullName,
                CreatedTime = DateTime.UtcNow,
                // CourseRegister:
                CourseRegisterId = Guid.NewGuid(),
                UserId = user.UserId,
                CourseId = course.UserItemId
            };
        }

        /// <summary>
        /// Tao mot payment cua user thanh toan khoa hoc
        /// </summary>
        /// <param name="user"> user thanh toan khoa hoc </param>
        /// <param name="course"> khoa hoc duoc thanh toann </param>
        /// <returns></returns>
        /// Created: PhucTV (31/3/24)
        /// Modified: None
        protected virtual CoursePayment CreateCoursePayment(ViewUser user, ViewCourse course)
        {
            return new CoursePayment()
            {
                // Entity:
                CreatedBy = user.FullName,
                CreatedTime = DateTime.UtcNow,
                // CoursePayment:
                CoursePaymentId = Guid.NewGuid(),
                UserId = user.UserId,
                CourseId = course.UserItemId,
                Fee = course.Fee
            };
        }

        public async Task<int> UserPayCourse(Guid userId, Guid courseId)
        {
            ViewUser? user = await DbContext.ViewUsers.FindAsync(userId);
            ViewCourse? course = await DbContext.ViewCourses.FindAsync(courseId);
            if (user == null || course == null) return 0;

            using var transaction = await DbContext.BeginTransaction();
            try
            {
                // Add Payment
                CoursePayment payment = CreateCoursePayment(user, course);
                DbContext.CoursePayments.Add(payment);

                // Add course register
                CourseRegister register = CreateCourseRegister(user, course);
                DbContext.CourseRegisters.Add(register);

                // Commit
                int effects = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return effects;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }
    }
}
