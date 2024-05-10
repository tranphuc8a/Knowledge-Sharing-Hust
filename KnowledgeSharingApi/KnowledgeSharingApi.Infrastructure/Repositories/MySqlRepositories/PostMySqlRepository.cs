using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
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
            var postsId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await DbContext.ViewPosts
                .Where(post => post.Privacy == EPrivacy.Public 
                        && postsId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                .OrderByDescending(post => post.CreatedTime)
                .Skip(offset).Take(limit)
                .ToListAsync();
            return posts;
        }

        public async Task<IEnumerable<ViewPost>> GetPostsOfCategory(string catName, int limit, int offset)
        {
            var postsId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await DbContext.ViewPosts
                .Where(post => postsId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
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


        #region Get Exactly

        public override async Task<IResponseUserItemModel?> GetExactlyResponseUserItemModel(Guid postId)
        {
            // Post
            Post? p = await DbContext.Posts.FindAsync(postId);
            if (p == null) return null;

            if (p.PostType == EPostType.Question)
            {
                // Question:
                ViewQuestion? question = await DbContext.ViewQuestions.FindAsync(postId);
                return question != null ? (ResponseQuestionModel)new ResponseQuestionModel().Copy(question) : null;
            }
            // Leson: 
            ViewLesson? lesson = await DbContext.ViewLessons.FindAsync(postId);
            return lesson != null ? (ResponseLessonModel)new ResponseLessonModel().Copy(lesson) : null;
        }

        public override async Task<Dictionary<Guid, IResponseUserItemModel?>> GetExactlyResponseUserItemModel(List<Guid> postIds)
        {
            Dictionary<Guid, IResponseUserItemModel?> res = postIds
                .ToDictionary(id => id, id => (IResponseUserItemModel?)null);

            // Get List lessonIds and questionsIds:
            var posts = await DbContext.Posts
                .Where(post => postIds.Contains(post.UserItemId))
                .ToListAsync();
            var lessonIds = posts
                .Where(post => post.PostType == EPostType.Lesson)
                .Select(post => post.UserItemId).ToList();
            var questionIds = posts.Select(u => u.UserItemId).Except(lessonIds).ToList();

            var vLessons = await DbContext.ViewLessons.Where(lesson => lessonIds.Contains(lesson.UserItemId)).ToListAsync();
            var vQuestions = await DbContext.ViewQuestions.Where(question => questionIds.Contains(question.UserItemId)).ToListAsync();

            // Return result:
            foreach (ViewLesson vLes in vLessons)
            {
                ResponseLessonModel resLes = new();
                res[vLes.UserItemId] = (ResponseLessonModel)resLes.Copy(vLes);
            }
            foreach (ViewQuestion vQues in vQuestions)
            {
                ResponseQuestionModel resQues = new();
                res[vQues.UserItemId] = (ResponseQuestionModel)resQues.Copy(vQues);
            }
            return res;
        }

        public override async Task<IUserItemView?> GetExactlyUserItem(Guid postId)
        {
            // Post
            Post? p = await DbContext.Posts.FindAsync(postId);
            if (p == null) return null;

            if (p.PostType == EPostType.Question)
            {
                // Question:
                return await DbContext.ViewQuestions.FindAsync(postId);
            }
            // Leson: 
            return await DbContext.ViewLessons.FindAsync(postId);
        }

        #endregion
    }
}
