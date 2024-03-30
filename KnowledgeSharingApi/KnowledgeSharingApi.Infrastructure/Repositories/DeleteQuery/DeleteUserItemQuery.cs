using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DeleteQuery
{
    public class DeleteUserItemQuery
    {
        public virtual void Execute(IDbContext dbContext, Guid id)
        {
            // Delete Stars:
            IQueryable<Star> stars = dbContext.Stars.Where(star => star.UserItemId == id);
            dbContext.Stars.RemoveRange(stars);
        }

        public virtual void Execute(IDbContext dbContext, IQueryable<Guid> ids)
        {
            // Delete stars:
            IQueryable<Star> stars = dbContext.Stars.Where(star => ids.Contains(star.UserItemId));
            dbContext.Stars.RemoveRange(stars);
        }
    }
}
