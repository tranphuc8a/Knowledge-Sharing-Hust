using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class PostMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Post>(dbContext), IPostRepository
    {
        public async Task<IEnumerable<ViewPost>> GetViewPost(int limit, int offset)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewPost>> GetByUserId(string userId)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Where(post => post.UserId.ToString() == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewPost>> GetPublicPosts(int limit, int offset)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Where(post => post.Privacy == EPrivacy.Public)
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewPost>> GetPublicPostsByUserId(string userId)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Where(post => post.Privacy == EPrivacy.Public && post.UserId.ToString() == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

    }
}
