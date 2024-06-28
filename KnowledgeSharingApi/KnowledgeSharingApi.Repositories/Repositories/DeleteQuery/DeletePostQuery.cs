using KnowledgeSharingApi.Infrastructures.Infrastructures.DeleteQuery;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.DeleteQuery
{
    public class DeletePostQuery : DeleteKnowledgeQuery
    {
        public override void Execute(IDbContext dbContext, Guid id)
        {
            base.Execute(dbContext, id);

            // Delete post history 
            dbContext.PostEditHistories.RemoveRange(
                dbContext.PostEditHistories.Where(p => p.PostId == id));
        }

        public override void Execute(IDbContext dbContext, IQueryable<Guid> ids)
        {
            base.Execute(dbContext, ids);

            // Delete post history 
            dbContext.PostEditHistories.RemoveRange(
                dbContext.PostEditHistories.Where(p => ids.Contains(p.PostId)));
        }
    }
}
