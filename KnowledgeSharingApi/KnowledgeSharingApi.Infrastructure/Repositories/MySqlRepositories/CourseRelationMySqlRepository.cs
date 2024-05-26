using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
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
        protected virtual CourseRegister CreateCourseRegister(User user, Course course)
        {
            return new CourseRegister()
            {
                // Entity:
                CreatedBy = user.Username,
                CreatedTime = DateTime.Now,
                // CourseRegister:
                CourseRegisterId = Guid.NewGuid(),
                UserId = user.UserId,
                CourseId = course.UserItemId
            };
        }

        public virtual async Task<int> ConfirmCourseRelation(Guid userRelationId)
        {
            CourseRelation? relation = await DbContext.CourseRelations.FindAsync(userRelationId);
            if (relation == null) return 0;

            Guid courseId = relation.CourseId;
            Guid userId = relation.CourseRelationType == ECourseRelationType.Invite ? relation.ReceiverId : relation.SenderId;

            User? user = await DbContext.Users.FindAsync(userId);
            Course? course = await DbContext.Courses.FindAsync(courseId);
            if (user == null || course == null) return 0;

            using var transaction = await DbContext.BeginTransaction();
            try
            {
                // Delete other relation between user and course:
                IQueryable<CourseRelation> listRelations = DbContext.CourseRelations
                    .Where(cre => cre.CourseId == courseId && (cre.SenderId == userId || cre.ReceiverId == userId));
                DbContext.CourseRelations.RemoveRange(listRelations);

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

        public virtual async Task<CourseRelation?> GetInvite(Guid receiverId, Guid courseId)
        {
            return await DbContext.CourseRelations
                .Where(cr => cr.CourseRelationType == ECourseRelationType.Invite &&
                    cr.CourseId == courseId && cr.ReceiverId == receiverId)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<CourseRelation?> GetRelation(Guid courseRelationId)
        {
            return await DbContext.CourseRelations.FindAsync(courseRelationId);
        }

        public virtual async Task<List<CourseRelation>> GetRelationsOfCourse(Guid courseId, ECourseRelationType relationType)
        {
            return await DbContext.CourseRelations
                .Where(cr => cr.CourseId == courseId && cr.CourseRelationType == relationType)
                .OrderByDescending(cr => cr.CreatedBy)
                .ToListAsync();
        }

        public virtual async Task<List<CourseRelation>> GetRelationsOfUser(Guid userId, ECourseRelationType relationType)
        {
            IQueryable<CourseRelation> query = DbContext.CourseRelations
                .Where(cr => cr.CourseRelationType == relationType)
                .OrderByDescending(cr => cr.CreatedBy);

            query = relationType == ECourseRelationType.Invite
                ? query.Where(cr => cr.ReceiverId == userId)
                : query.Where(cr => cr.SenderId == userId);

            return await query.ToListAsync();
        }

        public virtual async Task<CourseRelation?> GetRequest(Guid senderId, Guid courseId)
        {
            return await DbContext.CourseRelations
                .Where(cr => cr.CourseRelationType == ECourseRelationType.Request &&
                    cr.CourseId == courseId && cr.SenderId == senderId)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<ECourseRoleType> GetRole(Guid userId, Guid courseId)
        {
            // Strategy: Check level from high (owner) to low (inAccessible)
            // Check owner:
            ViewCourse? course = await DbContext.ViewCourses.FindAsync(courseId);
            if (course == null) return ECourseRoleType.NotAccessible;
            if (course.UserId == userId) return ECourseRoleType.Owner;

            // Check member:
            bool isMember = await DbContext.CourseRegisters
                .AnyAsync(cr => cr.UserId == userId && cr.CourseId == courseId);
            if (isMember) return ECourseRoleType.Member;

            // Check Guest:
            if (course.Privacy == EPrivacy.Public) return ECourseRoleType.Guest;
            bool isGuest = await DbContext.CourseRelations
                .AnyAsync(relation => relation.CourseRelationType == ECourseRelationType.Invite
                    && relation.ReceiverId == userId && relation.CourseId == courseId);
            if (isGuest) return ECourseRoleType.Guest;

            // other cases: Not accessible
            return ECourseRoleType.NotAccessible;
        }


        public async Task<Dictionary<Guid, CourseRoleTypeDto>> GetCourseRoleType(Guid userId, List<Guid> courseIds)
        {
            Dictionary<Guid, CourseRoleTypeDto> result = courseIds.Distinct().ToDictionary(
                id => id,
                id => new CourseRoleTypeDto()
                {
                    CourseRelationId = null,
                    CourseRoleType = ECourseRoleType.NotInRelation
                }
            );
            Dictionary<Guid, Course> courseDict = await DbContext.Courses
                .Where(c => courseIds.Contains(c.UserItemId))
                .ToDictionaryAsync(c => c.UserItemId, c => c);
            Dictionary<Guid, CourseRegister> registerDict = await DbContext.CourseRegisters
                .Where(cr => cr.UserId == userId && courseIds.Contains(cr.CourseId))
                .GroupBy(cr => cr.CourseId)
                .ToDictionaryAsync(group => group.Key, group => group.First());
            List<CourseRelation> relationList = await DbContext.CourseRelations
                .Where(crl => (crl.SenderId == userId || crl.ReceiverId == userId) && courseIds.Contains(crl.CourseId))
                .GroupBy(crl => crl.CourseId).Select(g => g.First())
                .ToListAsync();
            Dictionary<Guid, CourseRelation> invitedDict = relationList
                .Where(crl => crl.ReceiverId == userId)
                .ToDictionary(crl => crl.CourseId, crl => crl);
            Dictionary<Guid, CourseRelation> requestingDict = relationList
                .Where(crl => crl.SenderId == userId)
                .Distinct().ToDictionary(crl => crl.CourseId, crl => crl);

            foreach (Guid id in courseIds)
            {
                if (courseDict.TryGetValue(id, out Course? course))
                {
                    if (course.UserId == userId)
                    {
                        result[id].CourseRoleType = ECourseRoleType.Owner;
                        result[id].CourseRelationId = course.UserItemId;
                        continue;
                    }
                    if (registerDict.TryGetValue(id, out CourseRegister? value2))
                    {
                        result[id].CourseRoleType = ECourseRoleType.Member;
                        result[id].CourseRelationId = value2.CourseRegisterId;
                        continue;
                    }
                    if (invitedDict.TryGetValue(id, out CourseRelation? relation))
                    {
                        result[id].CourseRoleType = ECourseRoleType.Invited;
                        result[id].CourseRelationId = relation.CourseRelationId;
                        continue;
                    }
                    if (course.Privacy == EPrivacy.Private)
                    {
                        result[id].CourseRoleType = ECourseRoleType.NotAccessible;
                        result[id].CourseRelationId = null;
                        continue;
                    }
                    if (requestingDict.TryGetValue(id, out CourseRelation? relation2))
                    {
                        result[id].CourseRoleType = ECourseRoleType.Requesting;
                        result[id].CourseRelationId = relation2.CourseRelationId;
                        continue;
                    }

                }
            }
            return result;
        }
    }
}
