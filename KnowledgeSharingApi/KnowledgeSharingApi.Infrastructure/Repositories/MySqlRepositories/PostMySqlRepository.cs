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
        : BaseMySqlUserItemRepository<Post>(dbContext), IPostRepository
    {
        public async Task<IEnumerable<ViewPost>> GetViewPost(int limit, int offset)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewPost>> GetByUserId(Guid userId)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Where(post => post.UserId == userId)
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

        public async Task<IEnumerable<ViewPost>> GetPublicPostsByUserId(Guid userId)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewPost>> GetPublicPostsOfCategory(string catName, int limit, int offset)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await DbContext.ViewPosts
                .Where(post => post.Privacy == EPrivacy.Public 
                        && knowledgesId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewPost>> GetPostsOfCategory(string catName, int limit, int offset)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await DbContext.ViewPosts
                .Where(post => knowledgesId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public Task<IEnumerable<ViewPost>> GetPostsOfCategory(Guid myUId, string catName, int limit, int offset)
        {
            return GetPublicPostsOfCategory(catName, limit, offset);
        }

        public async Task<IEnumerable<ViewPost>> GetMarkedPosts(Guid userId)
        {
            IQueryable<ViewPost> query =
                from post in DbContext.ViewPosts
                join mark in DbContext.Marks
                    on post.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId && (
                    // userId có thể xem được post này:
                    // post là công khai
                    post.Privacy == EPrivacy.Public ||
                    // post là của chính user
                    post.UserId == userId
                )
                orderby mark.CreatedTime descending
                select post;
            return await query.ToListAsync();
        }

        protected override DbSet<Post> GetDbSet()
        {
            return DbContext.Posts;
        }
    }
}
