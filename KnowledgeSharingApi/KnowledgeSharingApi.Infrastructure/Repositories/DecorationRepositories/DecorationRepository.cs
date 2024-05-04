using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.DecorationRepositories
{
    public class DecorationRepository(
        IStarRepository starRepository,
        IKnowledgeRepository knowledgeRepository,
        ICategoryRepository categoryRepository,
        ICommentRepository commentRepository,
        IMarkRepository markRepository,
        IDbContext dbContext
        ) : BaseMySqlUserItemRepository<UserItem>(dbContext), IDecorationRepository
    {
        protected readonly IStarRepository StarRepository = starRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository = knowledgeRepository;
        protected readonly ICommentRepository CommentRepository = commentRepository;
        protected readonly ICategoryRepository CategoryRepository = categoryRepository;
        protected readonly IMarkRepository MarkRepository = markRepository;
        protected readonly int NumberOfTopComments = 20;

        #region Decorate functional methods

        protected virtual async Task<IResponseKnowledgeModel> DecorateResponseKnowledgeModel
            (Guid? myUid, IResponseKnowledgeModel knowledge)
        {
            // Get list top comments and list categories of knowledge
            // Top comments & NumberComments
            PaginationResponseModel<ViewComment> listComments =
                await KnowledgeRepository.GetListComments(knowledge.UserItemId, NumberOfTopComments, 0);
            knowledge.NumberComments = listComments.Total;
            knowledge.TopComments = listComments.Results.Select(com =>
            {
                ResponseCommentModel comment = new();
                comment.Copy(com);
                return comment;
            });

            // Lấy về danh sách categories
            List<Category> categories = (await CategoryRepository.GetByKnowledgeId(knowledge.UserItemId)).ToList();
            knowledge.Categories = categories;

            return knowledge;
        }


        protected virtual async Task<List<IResponseKnowledgeModel>> DecorateResponseKnowledgeModel
            (Guid? myUid, List<IResponseKnowledgeModel> knowledges)
        {
            // Attributes: myStar, totalStar, averageStar, number comments, top comments, categories, ismark
            List<Guid> knowledgeIds = knowledges.Select(lesson => lesson.UserItemId).ToList();


            Dictionary<Guid, int?>? myStars = null;
            Dictionary<Guid, bool>? isMarks = null;
            if (myUid != null)
            {
                // calculate myStar from myUid to all lessons
                myStars = await StarRepository.CalculateUserStars(myUid.Value, knowledgeIds);

                // is Mark
                isMarks = await MarkRepository.GetUserMarkListKnowledge(myUid.Value, knowledgeIds);
            }

            // calculate total stars to all lessons
            Dictionary<Guid, int> totalStars = await StarRepository.CalculateTotalStars(knowledgeIds);

            // calculate average stars to all lessons
            Dictionary<Guid, double?> averageStars = await StarRepository.CalculateAverageStars(knowledgeIds);

            // Number comments & Top comments
            Dictionary<Guid, PaginationResponseModel<ViewComment>?> topComments =
                await CommentRepository.GetListCommentsOfKnowledge(knowledgeIds, NumberOfTopComments);

            // List Categories
            Dictionary<Guid, List<Category>?> lsCategories =
                (await CategoryRepository.GetByKnowledgeId(knowledgeIds)).ToDictionary(
                    it => it.Key,
                    it => it.Value?.ToList()
                );

            foreach (IResponseKnowledgeModel knowledge in knowledges)
            {
                // Number comments & Top comments & List categories
                // await DecorateResponseKnowledgeModel(myUid, knowledge);
                knowledge.NumberComments = topComments[knowledge.UserItemId]?.Total ?? 0;
                knowledge.TopComments = topComments[knowledge.UserItemId] != null ?
                    topComments[knowledge.UserItemId]!.Results.Select(viewComment =>
                    {
                        ResponseCommentModel resComment = new();
                        resComment.Copy(viewComment);
                        return resComment;
                    }).ToList() : [];
                knowledge.Categories = lsCategories[knowledge.UserItemId] ?? [];
                if (myStars != null) 
                {
                    knowledge.MyStars = myStars[knowledge.UserItemId];
                }
                if (isMarks != null)
                {
                    knowledge.IsMarked = isMarks[knowledge.UserItemId];
                }
                knowledge.TotalStars = totalStars[knowledge.UserItemId];
                knowledge.AverageStars = averageStars[knowledge.UserItemId];
            }

            return knowledges;
        }

        protected override DbSet<UserItem> GetDbSet()
        {
            return DbContext.UserItems;
        }

        #endregion

        public virtual async Task<List<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid, List<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false)
        {
            // Atrributes: Course, Lesson
            Dictionary<Guid, ResponseCourseCardModel> mapCourse = [];
            Dictionary<Guid, ResponseLessonModel> mapLesson = [];
            if (isDecorateCourse)
            { // Get list course
                List<Guid> listCourseIds = participants.Select(p => p.CourseId).Distinct().ToList();
                List<ViewCourse> courses = await DbContext.ViewCourses.Where(c => listCourseIds.Contains(c.UserItemId)).ToListAsync();
                List<ResponseCourseCardModel> responseCourse = courses.Select(c => (ResponseCourseCardModel)new ResponseCourseCardModel().Copy(c)).ToList();
                foreach (var course in responseCourse) mapCourse[course.UserItemId] = course;
            }
            if (isDecorateLesson)
            { // get list lesson
                List<Guid> listLessonIds = participants.Select(p => p.LessonId).Distinct().ToList();
                List<ViewLesson> lessons = await DbContext.ViewLessons.Where(l => listLessonIds.Contains(l.UserItemId)).ToListAsync();
                List<ResponseLessonModel> responseLesson = await DecorateResponseLessonModel(myUid, lessons);
                foreach (var lesson in responseLesson) mapLesson[lesson.UserItemId] = lesson;
            }

            List<ResponseCourseLessonModel> res = participants.Select(part =>
            {
                ResponseCourseLessonModel item = (ResponseCourseLessonModel) new ResponseCourseLessonModel().Copy(part);
                item.Course = mapCourse[item.CourseId];
                item.Lesson = mapLesson[item.LessonId];
                return item;
            }).ToList();
            return res;
        }

        public virtual async Task<List<ResponseLessonModel>> DecorateResponseLessonModel(Guid? myUid, List<ViewLesson> lessons)
        {
            List<ResponseLessonModel> res = lessons.Select(lesson =>
            {
                ResponseLessonModel les = new();
                les.Copy(lesson);
                return les;
            }).ToList();
            await DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            return res;
        }

        public async Task<List<ResponseQuestionModel>> DecorateResponseQuestionModel(Guid? myUid, List<ViewQuestion> questions)
        {
            List<ResponseQuestionModel> res = questions.Select(question =>
            {
                ResponseQuestionModel ques = new();
                ques.Copy(question);
                return ques;
            }).ToList();
            await DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            return res;
        }

        public async Task<List<ResponseCourseModel>> DecorateResponseCourseModel(Guid? myUid, List<ViewCourse> courses)
        {
            List<ResponseCourseModel> res = courses.Select(course =>
            {
                ResponseCourseModel resCourse = new();
                resCourse.Copy(course);
                return resCourse;
            }).ToList();
            await DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            return res;
        }

    }
}
