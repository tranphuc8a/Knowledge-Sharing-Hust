using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using KnowledgeSharingApi.Repositories.Repositories.DeleteQuery;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class CourseMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<Course>(dbContext), ICourseRepository
    {
        public virtual async Task<ViewCourse> CheckExistedCourse(Guid courseId, string errorMessage)
        {
            return (ViewCourse)((await DbContext.ViewCourses.FindAsync(courseId))?.Clone()
                ?? throw new NotExistedEntityException(errorMessage));
        }

        // Override Delete Member
        public override async Task<int> Delete(Guid courseId)
        {
            using var transaction = await DbContext.BeginTransaction();
            try
            {
                new DeleteCourseQuery().Execute(DbContext, courseId);

                int effectRows = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return effectRows;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<List<ViewCourseRegister>> GetRegistersOfCourse(Guid courseId)
        {
            return await DbContext.ViewCourseRegisters
                .Where(cr => cr.CourseId == courseId)
                .OrderByDescending(cr => cr.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourseRegister>> GetRegistersOfUser(Guid userId)
        {
            return await DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == userId)
                .OrderByDescending(cr => cr.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetMarkedCoursesOfUser(Guid userId, PaginationDto pagination)
        {
            IQueryable<ViewCourse> listCourse =
                from viewCourse in DbContext.ViewCourses
                join mark in DbContext.Marks on viewCourse.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
                select viewCourse;

            return await ApplyPagination(
                    listCourse,
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetPublicViewCourse(PaginationDto pagination)
        {
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .Where(c => c.Privacy == EPrivacy.Public)
                    .OrderByDescending(c => c.CreatedTime),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetPublicViewCourseOfCategory(string catName, PaginationDto pagination)
        {
            IQueryable<Guid> listKnowledgeIds = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName)
                .Select(cat => cat.KnowledgeId);
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .Where(c => listKnowledgeIds.Contains(c.UserItemId) && c.Privacy == EPrivacy.Public)
                    .OrderByDescending(c => c.CreatedTime),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetPublicViewCourseOfUser(Guid userId)
        {
            return await DbContext.ViewCourses
                .Where(c => c.UserId == userId && c.Privacy == EPrivacy.Public)
                .OrderByDescending(c => c.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<ViewCourse?> GetViewCourse(Guid courseId)
        {
            return await DbContext.ViewCourses.FindAsync(courseId);
        }

        public virtual async Task<List<ViewCourse?>> GetViewCourse(Guid[] courseIds)
        {
            Dictionary<Guid, ViewCourse> vCourse = await DbContext.ViewCourses
                .Where(c => courseIds.Contains(c.UserItemId))
                .OrderByDescending(c => c.CreatedTime)
                .ToDictionaryAsync(
                    vc => vc.UserItemId,
                    vc => vc
                );
            return courseIds.ToList().Select(
                id =>
                {
                    if (vCourse.TryGetValue(id, out ViewCourse? vc))
                    {
                        return vc;
                    }
                    return null;
                }
            ).ToList();
        }

        public virtual async Task<List<ViewCourse>> GetViewCourse(PaginationDto pagination)
        {
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .OrderByDescending(c => c.CreatedTime),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetViewCourse(Guid myUid, PaginationDto pagination)
        {
            IQueryable<Guid> listRegisteredCourseIds = DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == myUid)
                .Select(cr => cr.CourseId);
            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == myUid && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);

            return await ApplyPagination(
                DbContext.ViewCourses
                .Where(vc => 
                    vc.Privacy == EPrivacy.Public ||
                    vc.UserId == myUid ||
                    listRegisteredCourseIds.Contains(vc.UserItemId) ||
                    listInvitedCourseIds.Contains(vc.UserItemId)
                )
                .OrderByDescending(c => c.CreatedTime),
                pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetViewCourseOfCategory(string catName, PaginationDto pagination)
        {
            IQueryable<Guid> listKnowledgeIds = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName).Select(cat => cat.KnowledgeId);
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .Where(course => listKnowledgeIds.Contains(course.UserItemId))
                    .OrderByDescending(c => c.CreatedTime),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetViewCourseOfCategory(Guid myUid, string catName, PaginationDto pagination)
        {
            // Lấy danh sách course mà myUid có tham gia
            IQueryable<Guid> listRegisteresCourseIds = DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == myUid).Select(cr => cr.CourseId);

            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == myUid && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);

            // Lấy danh sách knowledgeId có catName
            IQueryable<Guid> knowledgeIds = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName)
                .Select(cat => cat.KnowledgeId);

            // Lấy danh sách course thỏa mãn
            return await ApplyPagination(
                DbContext.ViewCourses
                .Where(c => knowledgeIds.Contains(c.UserItemId) && (
                    // User accessible:
                    // public
                    c.Privacy == EPrivacy.Public ||
                    // Owner:
                    c.UserId == myUid ||
                    // as a member:
                    listRegisteresCourseIds.Contains(c.UserItemId) ||
                    // Có lời mời invite:
                    listInvitedCourseIds.Contains(c.UserItemId)
                )).OrderByDescending(c => c.CreatedTime),
                pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetViewCourseOfUser(Guid userId)
        {
            return await DbContext.ViewCourses
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetViewCourseOfUser(Guid myUid, Guid userId)
        {
            // Lấy danh sách course mà myUid có tham gia
            IQueryable<Guid> listRegisteresCourseIds = DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == myUid).Select(cr => cr.CourseId);

            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == myUid && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);

            // Lấy danh sách course thỏa mãn
            return await DbContext.ViewCourses
                .Where(c => c.UserId == userId && (
                    // User accessible:
                    // public
                    c.Privacy == EPrivacy.Public ||
                    // Owner:
                    c.UserId == myUid ||
                    // as a member:
                    listRegisteresCourseIds.Contains(c.UserItemId) ||
                    // Có lời mời invite:
                    listInvitedCourseIds.Contains(c.UserItemId)
                )).OrderByDescending(c => c.CreatedTime)
                .ToListAsync();
        }

        public virtual async Task<ViewCourseRegister?> GetViewCourseRegister(Guid myUid, Guid courseId)
        {
            return await DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == myUid && cr.CourseId == courseId)
                .FirstOrDefaultAsync();
        }

        protected override DbSet<Course> GetDbSet()
        {
            return DbContext.Courses;
        }

        public virtual async Task<List<ViewCourseRegister>> GetRegistersOfCourse(Guid courseId, PaginationDto pagination)
        {
            return await ApplyPagination(DbContext.ViewCourseRegisters.Where(vcr => vcr.CourseId == courseId)
                .OrderByDescending(c => c.CreatedTime), pagination).ToListAsync();
        }

        public virtual async Task<List<ViewCourseRegister>> GetRegistersOfUser(Guid userId, PaginationDto pagination)
        {
            return await ApplyPagination(
                    DbContext.ViewCourseRegisters.Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.CreatedTime),
                    pagination
                ).ToListAsync();
        }

        public virtual async Task<List<ViewCourse>> GetPublicViewCourse()
        {
            return await DbContext.ViewCourses.Where(c => c.Privacy == EPrivacy.Public).ToListAsync();
        }

        #region Get T

        public virtual async Task<List<T>> GetMarkedCoursesOfUser<T>(Guid userId, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector)
        {
            IQueryable<ViewCourse> listCourse =
                from viewCourse in DbContext.ViewCourses
                join mark in DbContext.Marks on viewCourse.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
            select viewCourse;

            IQueryable<T> projectedQuery = listCourse.Select(projector);

            return await ApplyPagination(projectedQuery, pagination).ToListAsync();
        }

        public virtual async Task<List<T>> GetPublicViewCourse<T>(PaginationDto pagination, Expression<Func<ViewCourse, T>> projector)
        {
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .Where(c => c.Privacy == EPrivacy.Public)
                    .OrderByDescending(c => c.CreatedTime)
                    .Select(projector),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetPublicViewCourseOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector)
        {
            IQueryable<Guid> listKnowledgeIds = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName)
                .Select(cat => cat.KnowledgeId);
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .Where(c => listKnowledgeIds.Contains(c.UserItemId) && c.Privacy == EPrivacy.Public)
                    .OrderByDescending(c => c.CreatedTime)
                    .Select(projector),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetPublicViewCourseOfUser<T>(Guid userId, Expression<Func<ViewCourse, T>> projector)
        {
            return await DbContext.ViewCourses
                .Where(c => c.UserId == userId && c.Privacy == EPrivacy.Public)
                .OrderByDescending(c => c.CreatedTime)
                .Select(projector)
                .ToListAsync();
        }

        public virtual async Task<T?> GetViewCourse<T>(Guid courseId, Expression<Func<ViewCourse, T>> projector)
        {
            return await DbContext.ViewCourses.Where(item => item.UserItemId == courseId).Select(projector).FirstOrDefaultAsync();
        }

        public virtual async Task<List<T?>> GetViewCourse<T>(Guid[] courseIds, Expression<Func<ViewCourse, T>> projector)
        {
            Dictionary<Guid, T> vCourse = await DbContext.ViewCourses
                .Where(c => courseIds.Contains(c.UserItemId))
                .OrderByDescending(c => c.CreatedTime)
                .ToDictionaryAsync(
                    vc => vc.UserItemId,
                    projector.Compile()
                );
            return courseIds.ToList().Select(
                id =>
                {
                    if (vCourse.TryGetValue(id, out T? vc))
                    {
                        return vc;
                    }
                    return default;
                }
            ).ToList();
        }

        public virtual async Task<List<T>> GetViewCourse<T>(PaginationDto pagination, Expression<Func<ViewCourse, T>> projector)
        {
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .OrderByDescending(c => c.CreatedTime)
                    .Select(projector),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetViewCourse<T>(Guid myUid, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector)
        {
            IQueryable<Guid> listRegisteredCourseIds = DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == myUid)
                .Select(cr => cr.CourseId);
            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == myUid && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);

            return await ApplyPagination(
                DbContext.ViewCourses
                .Where(vc =>
                    vc.Privacy == EPrivacy.Public ||
                    vc.UserId == myUid ||
                    listRegisteredCourseIds.Contains(vc.UserItemId) ||
                    listInvitedCourseIds.Contains(vc.UserItemId)
                )
                .OrderByDescending(c => c.CreatedTime)
                .Select(projector),
                pagination)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetViewCourseOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector)
        {
            IQueryable<Guid> listKnowledgeIds = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName).Select(cat => cat.KnowledgeId);
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .Where(course => listKnowledgeIds.Contains(course.UserItemId))
                    .OrderByDescending(c => c.CreatedTime)
                    .Select(projector),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetViewCourseOfCategory<T>(Guid myUid, string catName, PaginationDto pagination, Expression<Func<ViewCourse, T>> projector)
        {
            // Lấy danh sách course mà myUid có tham gia
            IQueryable<Guid> listRegisteresCourseIds = DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == myUid).Select(cr => cr.CourseId);

            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == myUid && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);

            // Lấy danh sách knowledgeId có catName
            IQueryable<Guid> knowledgeIds = DbContext.ViewKnowledgeCategories
                .Where(cat => cat.CategoryName == catName)
                .Select(cat => cat.KnowledgeId);

            // Lấy danh sách course thỏa mãn
            return await ApplyPagination(
                DbContext.ViewCourses
                .Where(c => knowledgeIds.Contains(c.UserItemId) && (
                    // User accessible:
                    // public
                    c.Privacy == EPrivacy.Public ||
                    // Owner:
                    c.UserId == myUid ||
                    // as a member:
                    listRegisteresCourseIds.Contains(c.UserItemId) ||
                    // Có lời mời invite:
                    listInvitedCourseIds.Contains(c.UserItemId)
                ))
                .OrderByDescending(c => c.CreatedTime)
                .Select(projector),
                pagination)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetViewCourseOfUser<T>(Guid userId, Expression<Func<ViewCourse, T>> projector)
        {
            return await DbContext.ViewCourses
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedTime)
                .Select(projector)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetViewCourseOfUser<T>(Guid myUid, Guid userId, Expression<Func<ViewCourse, T>> projector)
        {
            // Lấy danh sách course mà myUid có tham gia
            IQueryable<Guid> listRegisteresCourseIds = DbContext.ViewCourseRegisters
                .Where(cr => cr.UserId == myUid).Select(cr => cr.CourseId);

            // Lấy danh sách course mà myUid được invite vào
            IQueryable<Guid> listInvitedCourseIds = DbContext.CourseRelations
                .Where(invite => invite.ReceiverId == myUid && invite.CourseRelationType == ECourseRelationType.Invite)
                .Select(invite => invite.CourseId);

            // Lấy danh sách course thỏa mãn
            return await DbContext.ViewCourses
                .Where(c => c.UserId == userId && (
                    // User accessible:
                    // public
                    c.Privacy == EPrivacy.Public ||
                    // Owner:
                    c.UserId == myUid ||
                    // as a member:
                    listRegisteresCourseIds.Contains(c.UserItemId) ||
                    // Có lời mời invite:
                    listInvitedCourseIds.Contains(c.UserItemId)
                ))
                .OrderByDescending(c => c.CreatedTime)
                .Select(projector)
                .ToListAsync();
        }


        public virtual async Task<List<T>> GetPublicViewCourse<T>(Expression<Func<ViewCourse, T>> projector)
        {
            return await DbContext.ViewCourses.Where(c => c.Privacy == EPrivacy.Public).Select(projector).ToListAsync();
        }

        #endregion

        public virtual async Task<Dictionary<Guid, CourseRoleTypeDto>> GetCourseRoleType(Guid userId, List<Guid> courseIds)
        {
            Dictionary<Guid, CourseRoleTypeDto> result = courseIds.Distinct().ToDictionary(
                id => id,
                id => new CourseRoleTypeDto()
                {
                    CourseRelationId = null,
                    CourseRoleType = ECourseRoleType.NotInRelation
                }
            );
            Dictionary<Guid, (Guid UserId, EPrivacy Privacy)> courseOwnerDict = await DbContext.Courses
                .Where(c => courseIds.Contains(c.UserItemId))
                .Select(c => new { c.UserItemId, c.UserId, c.Privacy })
                .ToDictionaryAsync(c => c.UserItemId, c => (c.UserId, c.Privacy));
            Dictionary<Guid, Guid> registerDict = await DbContext.CourseRegisters
                .Where(cr => cr.UserId == userId && courseIds.Contains(cr.CourseId))
                .Select(cr => new { cr.CourseId, cr.CourseRegisterId })
                .GroupBy(cr => cr.CourseId)
                .ToDictionaryAsync(group => group.Key, group => group.First().CourseRegisterId);
            var relationList = await DbContext.CourseRelations
                .Where(crl => (crl.SenderId == userId || crl.ReceiverId == userId) && courseIds.Contains(crl.CourseId))
                .Select(crl => new { crl.CourseId, crl.SenderId, crl.ReceiverId, crl.CourseRelationId })
                .GroupBy(crl => crl.CourseId).Select(g => g.First())
                .ToListAsync();
            Dictionary<Guid, Guid> invitedDict = relationList
                .Where(crl => crl.ReceiverId == userId)
                .ToDictionary(crl => crl.CourseId, crl => crl.CourseRelationId);
            Dictionary<Guid, Guid> requestingDict = relationList
                .Where(crl => crl.SenderId == userId)
                .Distinct().ToDictionary(crl => crl.CourseId, crl => crl.CourseRelationId);

            foreach (Guid id in courseIds)
            {
                if (courseOwnerDict.TryGetValue(id, out var course))
                {
                    if (course.UserId == userId)
                    {
                        result[id].CourseRoleType = ECourseRoleType.Owner;
                        result[id].CourseRelationId = id;
                        continue;
                    }
                    if (registerDict.TryGetValue(id, out Guid value2))
                    {
                        result[id].CourseRoleType = ECourseRoleType.Member;
                        result[id].CourseRelationId = value2;
                        continue;
                    }
                    if (invitedDict.TryGetValue(id, out Guid relation))
                    {
                        result[id].CourseRoleType = ECourseRoleType.Invited;
                        result[id].CourseRelationId = relation;
                        continue;
                    }
                    if (course.Privacy == EPrivacy.Private)
                    {
                        result[id].CourseRoleType = ECourseRoleType.NotAccessible;
                        result[id].CourseRelationId = null;
                        continue;
                    }
                    if (requestingDict.TryGetValue(id, out Guid relation2))
                    {
                        result[id].CourseRoleType = ECourseRoleType.Requesting;
                        result[id].CourseRelationId = relation2;
                        continue;
                    }

                }
            }
            return result;
        }
    }
}
