using Dapper;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
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
    public class PostMySqlRepository(IDbContext dbContext, ILessonRepository lessonRepository, IQuestionRepository questionRepository)
        : BaseMySqlUserItemRepository<Post>(dbContext), IPostRepository
    {
        protected readonly ILessonRepository LessonRepository = lessonRepository;
        protected readonly IQuestionRepository QuestionRepository = questionRepository;

        public virtual async Task<List<ViewPost>> GetViewPost(PaginationDto pagination)
        {
            List<ViewPost> posts = await ApplyPagination(
                DbContext.ViewPosts, pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewPost>> GetViewPost(Guid userId, PaginationDto pagination)
        {
            List<ViewPost> listLesson = (await LessonRepository.GetViewPost(userId, pagination)).Select(lesson =>
            {
                ViewPost p = new();
                p.Copy(lesson);
                return p;
            }).ToList();
            List<ViewPost> listQuestion = (await QuestionRepository.GetViewPost(userId, pagination)).Select(question =>
            {
                ViewPost p = new();
                p.Copy(question);
                return p;
            }).ToList();
            return ApplyLimitOffset(
                        listLesson.Concat(listQuestion)
                            .OrderByDescending(p => p.CreatedTime).ToList(), 
                        pagination.Limit, 
                        pagination.Offset
                    );
        }

        public virtual async Task<List<ViewPost>> GetByUserId(Guid userId)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Where(post => post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewPost>> GetByUserId(Guid userId, PaginationDto pagination)
        {
            List<ViewPost> posts = await ApplyPagination(
                    DbContext.ViewPosts
                    .Where(post => post.UserId == userId)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }


        public virtual async Task<List<ViewPost>> GetPublicPosts()
        {
            return await DbContext.ViewPosts.Where(p => p.Privacy == EPrivacy.Public).ToListAsync();
        }

        public virtual async Task<List<ViewPost>> GetPublicPosts(PaginationDto pagination)
        {
            List<ViewPost> posts = await ApplyPagination(
                    DbContext.ViewPosts
                    .Where(post => post.Privacy == EPrivacy.Public)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;

            //List<ViewPost> posts = (await
            //        DbContext.UserItems
            //        .OrderByDescending(post => post.CreatedTime)
            //        .Skip(pagination.Offset ?? 0).Take(pagination.Limit ?? 20)
            //    .ToListAsync())
            //    .Select(ui =>
            //    {
            //        ViewPost vp = new();
            //        vp.Copy(ui);
            //        return vp;
            //    }).ToList();
            //return posts;
        }

        public virtual async Task<List<ViewPost>> GetPublicPostsByUserId(Guid userId)
        {
            List<ViewPost> posts = await
                DbContext.ViewPosts
                .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewPost>> GetPublicPostsByUserId(Guid userId, PaginationDto pagination)
        {
            List<ViewPost> posts = await ApplyPagination(
                    DbContext.ViewPosts
                    .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewPost>> GetPublicPostsOfCategory(string catName, PaginationDto pagination)
        {
            var postsId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await ApplyPagination(
                    DbContext.ViewPosts
                    .Where(post => post.Privacy == EPrivacy.Public 
                            && postsId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewPost>> GetPostsOfCategory(string catName, PaginationDto pagination)
        {
            var postsId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await ApplyPagination(
                    DbContext.ViewPosts
                    .Where(post => postsId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewPost>> GetPostsOfCategory(Guid myUId, string catName, PaginationDto pagination)
        {
            List<ViewPost> listLesson = (await LessonRepository.GetPostsOfCategory(myUId, catName, pagination))
                .Select(lesson =>
                {
                    ViewPost p = new();
                    p.Copy(lesson);
                    return p;
                }).ToList();
            List<ViewPost> listQuestion = (await QuestionRepository.GetPostsOfCategory(myUId, catName, pagination))
                .Select(question =>
                {
                    ViewPost p = new();
                    p.Copy(question);
                    return p;
                }).ToList();
            return ApplyLimitOffset(
                        listLesson.Concat(listQuestion)
                            .OrderByDescending(p => p.CreatedTime).ToList(),
                        pagination.Limit,
                        pagination.Offset
                    );
        }

        public virtual async Task<List<ViewPost>> GetMarkedPosts(Guid userId)
        {
            IQueryable<ViewPost> query =
                from post in DbContext.ViewPosts
                join mark in DbContext.Marks
                    on post.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
                select post;
            return await query.ToListAsync();
        }

        public virtual async Task<List<ViewPost>> GetMarkedPosts(Guid userId, PaginationDto pagination)
        {
            IQueryable<ViewPost> query =
                from post in DbContext.ViewPosts
                join mark in DbContext.Marks
                    on post.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
                select post;
            query = ApplyPagination(query, pagination);
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
                .Distinct().ToDictionary(id => id, id => (IResponseUserItemModel?)null);

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
