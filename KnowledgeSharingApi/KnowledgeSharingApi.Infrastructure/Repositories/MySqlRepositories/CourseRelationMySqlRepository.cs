using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class CourseRelationMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<CourseRelation>(dbContext), ICourseRelationRepository
    {
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
                CreatedTime = DateTime.Now,
                // CourseRegister:
                CourseRegisterId = Guid.NewGuid(),
                UserId = user.UserId,
                CourseId = course.UserItemId
            };
        }

        public async Task<int> ConfirmCourseRelation(Guid userRelationId)
        {
            CourseRelation? relation = await DbContext.CourseRelations.FindAsync(userRelationId);
            if (relation == null) return 0;

            Guid courseId = relation.CourseId;
            Guid userId = relation.CourseRelationType == ECourseRelationType.Invite ? relation.ReceiverId : relation.SenderId;

            ViewUser? user = await DbContext.ViewUsers.FindAsync(userId);
            ViewCourse? course = await DbContext.ViewCourses.FindAsync(courseId);
            if (user == null || course == null) return 0;

            using var transaction = await DbContext.BeginTransaction();
            try
            {
                // Delete courseRelation
                DbContext.CourseRelations.Remove(relation);

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

        public async Task<CourseRelation?> GetInvite(Guid receiverId, Guid courseId)
        {
            return await DbContext.CourseRelations
                .Where(cr => cr.CourseRelationType == ECourseRelationType.Invite &&
                    cr.CourseId == courseId && cr.ReceiverId == receiverId)
                .FirstOrDefaultAsync();
        }

        public async Task<CourseRelation?> GetRelation(Guid courseRelationId)
        {
            return await DbContext.CourseRelations.FindAsync(courseRelationId);
        }

        public async Task<IEnumerable<CourseRelation>> GetRelationsOfCourse(Guid courseId, ECourseRelationType relationType)
        {
            return await DbContext.CourseRelations
                .Where(cr => cr.CourseId == courseId && cr.CourseRelationType == relationType)
                .OrderByDescending(cr => cr.CreatedBy)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseRelation>> GetRelationsOfUser(Guid userId, ECourseRelationType relationType)
        {
            IQueryable<CourseRelation> query = DbContext.CourseRelations
                .Where(cr => cr.CourseRelationType == relationType)
                .OrderByDescending(cr => cr.CreatedBy);

            query = relationType == ECourseRelationType.Invite
                ? query.Where(cr => cr.ReceiverId == userId)
                : query.Where(cr => cr.SenderId == userId);

            return await query.ToListAsync();
        }

        public async Task<CourseRelation?> GetRequest(Guid senderId, Guid courseId)
        {
            return await DbContext.CourseRelations
                .Where(cr => cr.CourseRelationType == ECourseRelationType.Request &&
                    cr.CourseId == courseId && cr.SenderId == senderId)
                .FirstOrDefaultAsync();
        }

        public async Task<ECourseRoleType> GetRole(Guid userId, Guid courseId)
        {
            // Strategy: Check level from high (owner) to low (inAccessible)
            // Check owner:
            ViewCourse? course = await DbContext.ViewCourses.FindAsync(courseId);
            if (course == null) return ECourseRoleType.NotAccessible;
            if (course.UserId == userId) return ECourseRoleType.Owner;

            // Check member:
            if (DbContext.CourseRegisters
                .Where(cr => cr.UserId == userId && cr.CourseId == courseId)
                .Any())
                return ECourseRoleType.Member;

            // Check Guest:
            if (course.Privacy == EPrivacy.Public) return ECourseRoleType.Guest;
            if (DbContext.CourseRelations
                .Where(relation => relation.CourseRelationType == ECourseRelationType.Invite
                    && relation.ReceiverId == userId && relation.CourseId == courseId)
                .Any())
                return ECourseRoleType.Guest;

            // other cases: Not accessible
            return ECourseRoleType.NotAccessible;
        }
    }
}
