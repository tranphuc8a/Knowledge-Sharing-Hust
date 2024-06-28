using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class StarMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Star>(dbContext), IStarRepository
    {
        public async Task<Dictionary<Guid, double?>> CalculateAverageStars(List<Guid> userItemsId)
        {
            // Lấy ra từ điển của UserItemId và giá trị trung bình của Stars.
            var averages = await DbContext.Stars
                .Where(star => userItemsId.Contains(star.UserItemId))
                .GroupBy(star => star.UserItemId)
                .Select(group => new { StarItemId = group.Key, AverageStars = group.Average(star => star.Stars) })
                .ToListAsync();

            // Chuyển đổi danh sách trung bình thành từ điển, với giá trị mặc định là null cho mọi giá trị.
            var result = userItemsId.Distinct().ToDictionary(id => id, id => (double?)null);

            // Đặt giá trị trung bình vào từ điển nếu có giá trị cho UserItemId.
            averages.ForEach(avg => result[avg.StarItemId] = avg.AverageStars);

            return result;
        }

        public async Task<Dictionary<Guid, int>> CalculateTotalStars(List<Guid> userItemsId)
        {
            // Lấy tổng số stars từ database đối với mỗi UserItemId
            var totalsQuery = DbContext.Stars
                .Where(star => userItemsId.Contains(star.UserItemId))
                .GroupBy(star => star.UserItemId)
                .Select(group => new
                {
                    UserItemId = group.Key,
                    TotalStars = group.Count()
                });

            // Chuyển đổi kết quả truy vấn thành dictionary và thực hiện truy vấn
            Dictionary<Guid, int> totals = await totalsQuery.ToDictionaryAsync(g => g.UserItemId, g => g.TotalStars);

            // Tạo dictionary kết quả, với mỗi UserItemId. Mặc định là 0 nếu không có giá trị tổng trong totals.
            Dictionary<Guid, int> result = userItemsId.Distinct().ToDictionary(
                id => id,
                id => totals.TryGetValue(id, out int value) ? value : 0
            );

            return result;
        }

        public async Task<Dictionary<Guid, int?>> CalculateUserStars(Guid userId, List<Guid> userItemsId)
        {
            var userstars = await DbContext.Stars
                .Where(star => star.UserId == userId && userItemsId.Contains(star.UserItemId))
                .Select(it => new { it.UserItemId, it.Stars})
                .ToListAsync();

            // Chuyển đổi danh sách trung bình thành từ điển, với giá trị mặc định là null cho mọi giá trị.
            Dictionary<Guid, int?> result = userItemsId.Distinct().ToDictionary(id => id, id => (int?)null);

            // Đặt giá trị star vào từ điển nếu có giá trị cho UserItemId.
            userstars.ForEach(ut => result[ut.UserItemId] = ut.Stars);

            return result;
        }

        public async Task<List<Star>> GetListStarOfUser(Guid userId)
        {
            return await DbContext.Stars.Where(star => star.UserId == userId)
                .OrderByDescending(star => star.Stars)
                .ThenByDescending(star => star.CreatedTime).ToListAsync();
        }

        public async Task<List<Star>> GetListStarOfUserItem(Guid userItemId)
        {
            return await DbContext.Stars.Where(star => star.UserItemId == userItemId).ToListAsync();
        }

        public async Task<List<Tuple<ViewComment, Star>>> GetStaredComments(Guid userId)
        {
            var userStars =
                from star in DbContext.Stars
                where star.UserId == userId
                select star;
            var comments =
                from comment in DbContext.ViewComments
                join star in userStars on comment.UserItemId equals star.UserItemId
                select new { Star = star, Comment = comment };
            var res = await comments.ToListAsync();
            return res.Select(item => Tuple.Create(item.Comment, item.Star)).ToList();
        }

        public async Task<List<Tuple<ViewCourse, Star>>> GetStaredCourses(Guid userId)
        {
            var userStars =
                from star in DbContext.Stars
                where star.UserId == userId
                select star;
            var courses =
                from course in DbContext.ViewCourses
                join star in userStars on course.UserItemId equals star.UserItemId
                select new { Star = star, Course = course };
            var res = await courses.ToListAsync();
            return res.Select(item => Tuple.Create(item.Course, item.Star)).ToList();
        }

        public async Task<List<Tuple<ViewLesson, Star>>> GetStaredLessons(Guid userId)
        {
            var userStars =
                from star in DbContext.Stars
                where star.UserId == userId
                select star;
            var lessons =
                from lesson in DbContext.ViewLessons
                join star in userStars on lesson.UserItemId equals star.UserItemId
                select new { Star = star, Lesson = lesson };
            var res = await lessons.ToListAsync();
            return res.Select(item => Tuple.Create(item.Lesson, item.Star)).ToList();
        }

        public async Task<List<Tuple<ViewPost, Star>>> GetStaredPosts(Guid userId)
        {
            var userStars =
                from star in DbContext.Stars
                where star.UserId == userId
                select star;
            var posts =
                from post in DbContext.ViewPosts
                join star in userStars on post.UserItemId equals star.UserItemId
                select new { Star = star, Post = post };
            var res = await posts.ToListAsync();
            return res.Select(item => Tuple.Create(item.Post, item.Star)).ToList();
        }

        public async Task<List<Tuple<ViewQuestion, Star>>> GetStaredQuestions(Guid userId)
        {
            var userStars =
                from star in DbContext.Stars
                where star.UserId == userId
                select star;
            var questions =
                from question in DbContext.ViewQuestions
                join star in userStars on question.UserItemId equals star.UserItemId
                select new { Star = star, Question = question };
            var res = await questions.ToListAsync();
            return res.Select(item => Tuple.Create(item.Question, item.Star)).ToList();
        }

        public async Task<Star?> GetStarOfUserAndUserItem(Guid userId, Guid itemId)
        {
            return (Star?)(await DbContext.Stars
                .Where(star => star.UserId == userId && star.UserItemId == itemId)
                .FirstOrDefaultAsync())?.Clone();
        }
    }
}
