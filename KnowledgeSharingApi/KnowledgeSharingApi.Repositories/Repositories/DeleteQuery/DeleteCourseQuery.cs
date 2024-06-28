using KnowledgeSharingApi.Infrastructures.Infrastructures.DeleteQuery;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.DeleteQuery
{
    public class DeleteCourseQuery : DeleteKnowledgeQuery
    {
        public override void Execute(IDbContext dbContext, Guid id)
        {
            base.Execute(dbContext, id);

            // Delete list question
            IQueryable<Guid> listQuestionToDeleteIds = dbContext.Questions.Where(q => q.CourseId == id).Select(q => q.UserItemId);
            // Delete

            // Delete list course lesson
            dbContext.CourseLessons.RemoveRange(
                dbContext.CourseLessons.Where(cl => cl.CourseId == id));

            // Delete course payment
            dbContext.CoursePayments.RemoveRange(
                dbContext.CoursePayments.Where(pay => pay.CourseId == id));

            // Delete course relation
            dbContext.CourseRegisters.RemoveRange(
                dbContext.CourseRegisters.Where(cr => cr.CourseId == id));
            dbContext.CourseRelations.RemoveRange(
                dbContext.CourseRelations.Where(cr => cr.CourseId == id));

            // delete course:
            dbContext.Courses.RemoveRange(dbContext.Courses.Where(c => c.UserItemId == id));
        }

        public override void Execute(IDbContext dbContext, IQueryable<Guid> ids)
        {
            base.Execute(dbContext, ids);

            // Delete list question
            IQueryable<Guid> listQuestionToDeleteIds = dbContext.Questions
                .Where(q => q.CourseId != null && ids.Contains(q.CourseId.Value))
                .Select(q => q.UserItemId);
            // Delete

            // Delete list course lesson
            dbContext.CourseLessons.RemoveRange(
                dbContext.CourseLessons.Where(cl => ids.Contains(cl.CourseId)));

            // Delete course payment
            dbContext.CoursePayments.RemoveRange(
                dbContext.CoursePayments.Where(pay => ids.Contains(pay.CourseId)));

            // Delete course relation
            dbContext.CourseRegisters.RemoveRange(
                dbContext.CourseRegisters.Where(cr => ids.Contains(cr.CourseId)));
            dbContext.CourseRelations.RemoveRange(
                dbContext.CourseRelations.Where(cr => ids.Contains(cr.CourseId)));

            // delete course:
            dbContext.Courses.RemoveRange(dbContext.Courses.Where(c => ids.Contains(c.UserItemId)));
        }
    }
}
