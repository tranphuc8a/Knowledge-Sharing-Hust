using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DeleteQuery
{
    public class DeleteQuestionQuery : DeletePostQuery
    {
        public override void Execute(IDbContext dbContext, Guid id)
        {
            base.Execute(dbContext, id);

            // Delete Question
            dbContext.Questions.RemoveRange(
                dbContext.Questions.Where(q => q.UserItemId == id));
        }

        public override void Execute(IDbContext dbContext, IQueryable<Guid> ids)
        {
            base.Execute(dbContext, ids);

            // Delete Question
            dbContext.Questions.RemoveRange(
                dbContext.Questions.Where(q => ids.Contains(q.UserItemId)));
        }
    }
}
