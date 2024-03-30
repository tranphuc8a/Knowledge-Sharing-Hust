using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DeleteQuery
{
    public class DeleteLessonQuery : DeletePostQuery
    {
        public override void Execute(IDbContext dbContext, Guid id)
        {
            base.Execute(dbContext, id);

            // Delete CourseLessons
            dbContext.CourseLessons.RemoveRange(
                dbContext.CourseLessons.Where(cl => cl.LessonId == id));

            // Delete Lesson
            dbContext.Lessons.RemoveRange(
                dbContext.Lessons.Where(l => l.UserItemId == id));
        }

        public override void Execute(IDbContext dbContext, IQueryable<Guid> ids)
        {
            base.Execute(dbContext, ids);

            // Delete CourseLessons
            dbContext.CourseLessons.RemoveRange(
                dbContext.CourseLessons.Where(cl => ids.Contains(cl.LessonId)));

            // Delete Lesson
            dbContext.Lessons.RemoveRange(
                dbContext.Lessons.Where(l => ids.Contains(l.UserItemId)));
        }
    }
}
