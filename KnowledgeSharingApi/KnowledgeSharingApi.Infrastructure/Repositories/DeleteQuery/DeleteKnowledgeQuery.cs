using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore.Internal;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DeleteQuery
{
    public class DeleteKnowledgeQuery : DeleteUserItemQuery
    {
        public override void Execute(IDbContext dbContext, Guid id)
        {
            Execute(dbContext, id);

            // Delete study progress
            dbContext.StudyProgresses.RemoveRange(
                dbContext.StudyProgresses.Where(pro => pro.KnowledgeId == id));

            // Delete list comments of knowledge
            IQueryable<Guid> commentIds = dbContext.Comments
                .Where(com => com.KnowledgeId == id).Select(com => com.UserItemId);
            new DeleteCommentQuery().Execute(dbContext, commentIds);

            // Delete list categories of knowledge
            dbContext.KnowledgeCategories.RemoveRange(
                dbContext.KnowledgeCategories.Where(kn => kn.KnowledgeId == id));

            // Delete list save
            dbContext.Marks.RemoveRange(
                dbContext.Marks.Where(m => m.KnowledgeId == id));
        }

        public override void Execute(IDbContext dbContext, IQueryable<Guid> ids)
        {
            base.Execute(dbContext, ids);

            // Delete study progress
            dbContext.StudyProgresses.RemoveRange(
                dbContext.StudyProgresses.Where(pro => ids.Contains(pro.KnowledgeId)));

            // Delete list comments of knowledge
            IQueryable<Guid> listCommendIds = dbContext.Comments
                .Where(com => ids.Contains(com.KnowledgeId)).Select(com => com.UserItemId);
            new DeleteCommentQuery().Execute(dbContext, listCommendIds);

            // Delete list categories of knowledge
            dbContext.KnowledgeCategories.RemoveRange(
                dbContext.KnowledgeCategories.Where(kn => ids.Contains(kn.KnowledgeId)));

            // Delete list save
            dbContext.Marks.RemoveRange(
                dbContext.Marks.Where(m => ids.Contains(m.KnowledgeId)));
        }
    }
}
