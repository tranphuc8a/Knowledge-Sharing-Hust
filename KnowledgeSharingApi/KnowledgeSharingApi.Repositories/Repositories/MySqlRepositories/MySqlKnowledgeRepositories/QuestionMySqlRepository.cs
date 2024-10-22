﻿using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class QuestionMySqlRepository(IDbContext dbContext)
        : BaseMySqlUserItemRepository<Question>(dbContext), IQuestionRepository
    {
        public virtual async Task<ViewQuestion> CheckExistedQuestion(Guid questionId, string errorMessage)
        {
            return (ViewQuestion)((await DbContext.ViewQuestions
                .Where(question => question.UserItemId == questionId)
                .FirstOrDefaultAsync())?.Clone()
                ?? throw new NotExistedEntityException(errorMessage));
        }

        public override async Task<int> Delete(Guid questionId)
        {
            if (!DbContext.UserItems.Any(item => item.UserItemId == questionId))
                return 0;

            using IDbContextTransaction transaction = await DbContext.BeginTransaction();
            try
            {
                DbContext.KnowledgeCategories.RemoveRange(DbContext.KnowledgeCategories.Where(item => item.KnowledgeId == questionId));
                DbContext.Comments.RemoveRange(DbContext.Comments.Where(item => item.KnowledgeId == questionId));
                DbContext.Marks.RemoveRange(DbContext.Marks.Where(item => item.KnowledgeId == questionId));
                DbContext.Stars.RemoveRange(DbContext.Stars.Where(item => item.UserItemId == questionId));

                DbContext.Questions.RemoveRange(DbContext.Questions.Where(item => item.UserItemId == questionId));
                //DbContext.Posts.RemoveRange(DbContext.Posts.Where(item => item.UserItemId == questionId));
                //DbContext.Knowledges.RemoveRange(DbContext.Knowledges.Where(item => item.UserItemId == questionId));
                //DbContext.UserItems.RemoveRange(DbContext.UserItems.Where(item => item.UserItemId == questionId));

                int rows = await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return rows;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await transaction.RollbackAsync();
                return 0;
            }
        }

        public override async Task<Guid?> Insert(Guid questionId, Question question)
        {
            question.UserItemId = questionId;
            question.CreatedTime ??= DateTime.UtcNow;
            DbContext.Questions.Add(question);
            int rows = await DbContext.SaveChangesAsync();
            return rows > 0 ? questionId : null;
        }

        public override async Task<int> Update(Guid questionId, Question updatedQuestion)
        {
            Question questionToUpdate = await DbContext.Questions.FindAsync(questionId)
                ?? throw new Exception("Question not found.");

            updatedQuestion.UserItemId = questionId;
            updatedQuestion.ModifiedTime = DateTime.UtcNow;

            DbContext.Entry(questionToUpdate).CurrentValues.SetValues(updatedQuestion);

            return await DbContext.SaveChangesAsync();
        }

        #region Get ViewQuestion
        public virtual async Task<List<ViewQuestion>> GetByUserId(Guid userId)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }


        public virtual async Task<List<ViewQuestion>> GetByUserId(Guid userId, PaginationDto pagination)
        {
            List<ViewQuestion> posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.UserId == userId)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetPublicPosts(PaginationDto pagination)
        {
            List<ViewQuestion> posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.Privacy == EPrivacy.Public)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetPublicPostsByUserId(Guid userId)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetPublicPostsByUserId(Guid userId, PaginationDto pagination)
        {
            List<ViewQuestion> posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetQuestionInCourse(Guid courseid)
        {
            List<ViewQuestion> posts = await
                DbContext.ViewQuestions
                .Where(post => post.CourseId != null && post.CourseId == courseid)
                .OrderByDescending(post => post.CreatedTime)
                .ToListAsync();
            return posts;
        }


        public virtual async Task<ViewQuestion?> GetQuestionDetail(Guid questionId)
        {
            return await DbContext.ViewQuestions
                .Where(ques => ques.UserItemId == questionId)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<List<ViewQuestion>> GetViewPost(PaginationDto pagination)
        {
            return await ApplyPagination(
                    DbContext.ViewQuestions
                    .OrderByDescending(question => question.CreatedTime),
                    pagination)
                .ToListAsync();
        }

        public virtual async Task<List<ViewQuestion>> GetViewPost(Guid userId, PaginationDto pagination)
        {
            // Lấy danh sách CourseId mà User đã đăng ký
            var registeredCourseIds = DbContext.CourseRegisters
                .Where(c => c.UserId == userId)
                .Select(c => c.CourseId)
                .Distinct()
                .ToList(); // Dùng ToList để tải kết quả về và tránh query lại nhiều lần

            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post =>
                            // myUid có thể truy cập post:
                            // post là bài thảo luận của chính mình
                            post.UserId == userId ||
                            // hoặc post là bài thảo luận public
                            post.Privacy == EPrivacy.Public ||
                            // hoặc post là câu hỏi trong khóa học mà myUid có tham gia
                            post.CourseId != null && registeredCourseIds.Contains(post.CourseId.Value)

                    )
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();

            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetPublicPostsOfCategory(string catName, PaginationDto pagination)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.Privacy == EPrivacy.Public) // công khai
                    .Where(post => knowledgesId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetPostsOfCategory(string catName, PaginationDto pagination)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();

            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => knowledgesId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();

            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetPostsOfCategory(Guid myUId, string catName, PaginationDto pagination)
        {
            // Danh sach cac knowledge co cateName
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();

            // Lấy danh sách CourseId mà User đã đăng ký
            var registeredCourseIds = DbContext.CourseRegisters
                .Where(c => c.UserId == myUId)
                .Select(c => c.CourseId)
                .Distinct()
                .ToList(); // Dùng ToList để tải kết quả về và tránh query lại nhiều lần

            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => knowledgesId.Contains(post.UserItemId) &&
                        (   // myUid có thể truy cập post:
                            // post là bài thảo luận của chính mình
                            post.UserId == myUId ||
                            // hoặc post là bài thảo luận public
                            post.Privacy == EPrivacy.Public ||
                            // hoặc post là câu hỏi trong khóa học mà myUid có tham gia
                            post.CourseId != null && registeredCourseIds.Contains(post.CourseId.Value)
                        )
                    )
                    .OrderByDescending(post => post.CreatedTime),
                    pagination)
                .ToListAsync();

            return posts;
        }

        public virtual async Task<List<ViewQuestion>> GetMarkedPosts(Guid userId)
        {
            var query =
                from post in DbContext.ViewQuestions
                join mark in DbContext.Marks
                    on post.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
                select post;

            // Thực thi truy vấn và trả về kết quả
            return await query.ToListAsync();
        }

        public virtual async Task<List<ViewQuestion>> GetMarkedPosts(Guid userId, PaginationDto pagination)
        {
            var query =
                from post in DbContext.ViewQuestions
                join mark in DbContext.Marks
                    on post.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
                select post;

            // Apply Pagination
            query = ApplyPagination(query, pagination);

            // Thực thi truy vấn và trả về kết quả
            return await query.ToListAsync();
        } 

        public virtual async Task<List<ViewQuestion>> GetPublicPosts()
        {
            return await DbContext.ViewQuestions.Where(q => q.Privacy == EPrivacy.Public).ToListAsync();
        }
        #endregion

        protected override DbSet<Question> GetDbSet()
        {
            return DbContext.Questions;
        }


        #region Get T
        public virtual async Task<List<T>> GetByUserId<T>(Guid userId, Expression<Func<ViewQuestion, T>> projector)
        {
            List<T> posts = await
                DbContext.ViewQuestions
                .Where(post => post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .Select(projector)
                .ToListAsync();
            return posts;
        }


        public virtual async Task<List<T>> GetByUserId<T>(Guid userId, PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            List<T> posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.UserId == userId)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<T>> GetPublicPosts<T>(PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            List<T> posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.Privacy == EPrivacy.Public)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<T>> GetPublicPostsByUserId<T>(Guid userId, Expression<Func<ViewQuestion, T>> projector)
        {
            List<T> posts = await
                DbContext.ViewQuestions
                .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                .OrderByDescending(post => post.CreatedTime)
                .Select(projector)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<T>> GetPublicPostsByUserId<T>(Guid userId, PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            List<T> posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.Privacy == EPrivacy.Public && post.UserId == userId)
                    .OrderByDescending(post => post.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<T>> GetQuestionInCourse<T>(Guid courseid, Expression<Func<ViewQuestion, T>> projector)
        {
            List<T> posts = await
                DbContext.ViewQuestions
                .Where(post => post.CourseId != null && post.CourseId == courseid)
                .OrderByDescending(post => post.CreatedTime)
                .Select(projector)
                .ToListAsync();
            return posts;
        }


        public virtual async Task<T?> GetQuestionDetail<T>(Guid questionId, Expression<Func<ViewQuestion, T>> projector)
        {
            return await DbContext.ViewQuestions
                .Where(ques => ques.UserItemId == questionId)
                .Select(projector)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<List<T>> GetViewPost<T>(PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            return await ApplyPagination(
                    DbContext.ViewQuestions
                    .OrderByDescending(question => question.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetViewPost<T>(Guid userId, PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            // Lấy danh sách CourseId mà User đã đăng ký
            var registeredCourseIds = DbContext.CourseRegisters
                .Where(c => c.UserId == userId)
                .Select(c => c.CourseId)
                .Distinct()
                .ToList(); // Dùng ToList để tải kết quả về và tránh query lại nhiều lần

            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post =>
                            // myUid có thể truy cập post:
                            // post là bài thảo luận của chính mình
                            post.UserId == userId ||
                            // hoặc post là bài thảo luận public
                            post.Privacy == EPrivacy.Public ||
                            // hoặc post là câu hỏi trong khóa học mà myUid có tham gia
                            post.CourseId != null && registeredCourseIds.Contains(post.CourseId.Value)

                    )
                    .OrderByDescending(post => post.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();

            return posts;
        }

        public virtual async Task<List<T>> GetPublicPostsOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();
            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => post.Privacy == EPrivacy.Public) // công khai
                    .Where(post => knowledgesId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                    .OrderByDescending(post => post.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();
            return posts;
        }

        public virtual async Task<List<T>> GetPostsOfCategory<T>(string catName, PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();

            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => knowledgesId.Contains(post.UserItemId)) // Lọc post dựa vào danh sách ID đã lấy
                    .OrderByDescending(post => post.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();

            return posts;
        }

        public virtual async Task<List<T>> GetPostsOfCategory<T>(Guid myUId, string catName, PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            // Danh sach cac knowledge co cateName
            var knowledgesId = DbContext.ViewKnowledgeCategories
                .Where(k => k.CategoryName == catName)
                .Select(k => k.KnowledgeId)
                .Distinct();

            // Lấy danh sách CourseId mà User đã đăng ký
            var registeredCourseIds = DbContext.CourseRegisters
                .Where(c => c.UserId == myUId)
                .Select(c => c.CourseId)
                .Distinct()
                .ToList(); // Dùng ToList để tải kết quả về và tránh query lại nhiều lần

            var posts = await ApplyPagination(
                    DbContext.ViewQuestions
                    .Where(post => knowledgesId.Contains(post.UserItemId) &&
                        (   // myUid có thể truy cập post:
                            // post là bài thảo luận của chính mình
                            post.UserId == myUId ||
                            // hoặc post là bài thảo luận public
                            post.Privacy == EPrivacy.Public ||
                            // hoặc post là câu hỏi trong khóa học mà myUid có tham gia
                            post.CourseId != null && registeredCourseIds.Contains(post.CourseId.Value)
                        )
                    )
                    .OrderByDescending(post => post.CreatedTime),
                    pagination).Select(projector)
                .ToListAsync();

            return posts;
        }

        public virtual async Task<List<T>> GetMarkedPosts<T>(Guid userId, Expression<Func<ViewQuestion, T>> projector)
        {
            var query =
                from post in DbContext.ViewQuestions
                join mark in DbContext.Marks
                    on post.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
                select post;

            var projected = query.Select(projector);
            // Thực thi truy vấn và trả về kết quả
            return await projected.ToListAsync();
        }

        public virtual async Task<List<T>> GetMarkedPosts<T>(Guid userId, PaginationDto pagination, Expression<Func<ViewQuestion, T>> projector)
        {
            var query =
                from post in DbContext.ViewQuestions
                join mark in DbContext.Marks
                    on post.UserItemId equals mark.KnowledgeId
                where mark.UserId == userId
                orderby mark.CreatedTime descending
                select post;

            // Apply Pagination
            var projected = ApplyPagination(query, pagination).Select(projector);

            // Thực thi truy vấn và trả về kết quả
            return await projected.ToListAsync();
        }

        public virtual async Task<List<T>> GetPublicPosts<T>(Expression<Func<ViewQuestion, T>> projector)
        {
            return await DbContext.ViewQuestions.Where(q => q.Privacy == EPrivacy.Public).Select(projector).ToListAsync();
        }

        #endregion
    }
}
