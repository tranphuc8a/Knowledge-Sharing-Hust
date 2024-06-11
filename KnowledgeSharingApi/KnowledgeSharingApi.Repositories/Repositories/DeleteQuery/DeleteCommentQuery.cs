using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.DeleteQuery
{
    public class DeleteCommentQuery : DeleteUserItemQuery
    {
        public override void Execute(IDbContext dbContext, Guid id)
        {
            base.Execute(dbContext, id);
            IQueryable<Comment> lsComments = dbContext.Comments.Where(com => com.UserItemId == id);
            dbContext.Comments.RemoveRange(lsComments);
        }

        public override void Execute(IDbContext dbContext, IQueryable<Guid> ids)
        {
            base.Execute(dbContext, ids);
            IQueryable<Comment> lsComments = dbContext.Comments.Where(com => ids.Contains(com.UserItemId));
            dbContext.Comments.RemoveRange(lsComments);
        }
    }
}
