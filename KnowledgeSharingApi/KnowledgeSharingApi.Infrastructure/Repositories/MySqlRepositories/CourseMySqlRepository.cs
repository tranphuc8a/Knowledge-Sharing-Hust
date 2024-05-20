using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.DeleteQuery;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class CourseMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<Course>(dbContext), ICourseRepository
    {
        public virtual async Task<ViewCourse> CheckExistedCourse(Guid courseId, string errorMessage)
        {
            return (ViewCourse) ((await DbContext.ViewCourses.FindAsync(courseId))?.Clone()
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

        public virtual async Task<List<ViewCourse>> GetMarkedCoursesOfUse(Guid userId, PaginationDto pagination)
        {
            IQueryable<Guid> listMarkedKnowledgeIds = DbContext.Marks
                .Where(m => m.UserId == userId).Select(m => m.KnowledgeId);
            return await ApplyPagination(
                    DbContext.ViewCourses
                    .Where(c => listMarkedKnowledgeIds.Contains(c.UserItemId))
                    .OrderByDescending(c => c.CreatedTime),
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
                .Where(vc => (
                    vc.Privacy == EPrivacy.Public ||
                    vc.UserId == myUid ||
                    listRegisteredCourseIds.Contains(vc.UserItemId) ||
                    listInvitedCourseIds.Contains(vc.UserItemId)
                ))
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
                    (c.Privacy == EPrivacy.Public) ||
                    // Owner:
                    (c.UserId == myUid) ||
                    // as a member:
                    (listRegisteresCourseIds.Contains(c.UserItemId) ||
                    // Có lời mời invite:
                    (listInvitedCourseIds.Contains(c.UserItemId))
                ))).OrderByDescending(c => c.CreatedTime),
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
                    (c.Privacy == EPrivacy.Public) ||
                    // Owner:
                    (c.UserId == myUid) ||
                    // as a member:
                    (listRegisteresCourseIds.Contains(c.UserItemId) ||
                    // Có lời mời invite:
                    (listInvitedCourseIds.Contains(c.UserItemId))
                ))).OrderByDescending(c => c.CreatedTime)
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
    }
}
