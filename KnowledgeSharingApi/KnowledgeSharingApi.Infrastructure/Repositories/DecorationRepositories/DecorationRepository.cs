using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
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
        IPostRepository postRepository,
        IMarkRepository markRepository,
        IUserRelationRepository userRelationRepository,
        IUserItemRepository userItemRepository,
        IUserRepository userRepository,
        IDbContext dbContext
        ) : BaseMySqlUserItemRepository<UserItem>(dbContext), IDecorationRepository
    {
        protected readonly IStarRepository StarRepository = starRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository = knowledgeRepository;
        protected readonly ICommentRepository CommentRepository = commentRepository;
        protected readonly ICategoryRepository CategoryRepository = categoryRepository;
        protected readonly IMarkRepository MarkRepository = markRepository;
        protected readonly IPostRepository PostRepository = postRepository;
        protected readonly IUserItemRepository UserItemRepository = userItemRepository;
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IUserRelationRepository UserRelationRepository = userRelationRepository;
        protected readonly int NumberOfTopComments = 20;

        #region Decorate functional methods

        //protected virtual async Task<IResponseKnowledgeModel> DecorateResponseKnowledgeModel
        //    (Guid? myUid, IResponseKnowledgeModel knowledge)
        //{
        //     Get list top comments and list categories of knowledge
        //     Top comments & NumberComments
        //    PaginationResponseModel<ViewComment> listComments =
        //        await KnowledgeRepository.GetListComments(knowledge.UserItemId, NumberOfTopComments, 0);
        //    knowledge.NumberComments = listComments.Total;
        //    knowledge.TopComments = listComments.Results.Select(com =>
        //    {
        //        ResponseCommentModel comment = new();
        //        comment.Copy(com);
        //        return comment;
        //    });

        //     Lấy về danh sách categories
        //    List<Category> categories = (await CategoryRepository.GetByKnowledgeId(knowledge.UserItemId)).ToList();
        //    knowledge.Categories = categories;

        //    return knowledge;
        //}


        protected virtual async Task<List<IResponseUserItemModel>> DecorateResponseUserItemModel(Guid? myUid, List<IResponseUserItemModel> userItems)
        {
            // Attributes: myStar, totalStar, averageStar
            List<Guid> uiid = userItems.Select(lesson => lesson.UserItemId).ToList();

            Dictionary<Guid, int?>? myStars = null;
            if (myUid != null)
            {
                // calculate myStar from myUid to all lessons
                myStars = await StarRepository.CalculateUserStars(myUid.Value, uiid);
            }

            // calculate total stars to all lessons
            Dictionary<Guid, int> totalStars = await StarRepository.CalculateTotalStars(uiid);

            // calculate average stars to all lessons
            Dictionary<Guid, double?> averageStars = await StarRepository.CalculateAverageStars(uiid);

            foreach (IResponseUserItemModel useritem in userItems)
            {
                if (myStars != null)
                {
                    useritem.MyStars = myStars[useritem.UserItemId];
                }
                useritem.TotalStars = totalStars[useritem.UserItemId];
                useritem.AverageStars = averageStars[useritem.UserItemId];
            }

            return userItems;
        }

        protected virtual async Task<List<IResponseKnowledgeModel>> DecorateResponseKnowledgeModel
            (Guid? myUid, List<IResponseKnowledgeModel> knowledges)
        {
            await DecorateResponseUserItemModel(myUid, knowledges.OfType<IResponseUserItemModel>().ToList());

            // Attributes: number comments, top comments, categories, ismark
            List<Guid> knowledgeIds = knowledges.Select(lesson => lesson.UserItemId).ToList();

            Dictionary<Guid, bool>? isMarks = null;
            if (myUid != null)
            {
                // is Mark
                isMarks = await MarkRepository.GetUserMarkListKnowledge(myUid.Value, knowledgeIds);
            }
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
                if (isMarks != null)
                {
                    knowledge.IsMarked = isMarks[knowledge.UserItemId];
                };
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
            Dictionary<Guid, IResponseCourseModel?> mapCourse = [];
            Dictionary<Guid, IResponseLessonModel?> mapLesson = [];
            if (isDecorateCourse)
            { // Get list course
                List<Guid> listCourseIds = participants.Select(p => p.CourseId).Distinct().ToList();
                List<ViewCourse> courses = await DbContext.ViewCourses.Where(c => listCourseIds.Contains(c.UserItemId)).ToListAsync();
                List<IResponseCourseModel> responseCourse = await DecorateResponseCourseModel(myUid, courses);
                mapCourse = listCourseIds.ToDictionary(
                    id => id,
                    id => (IResponseCourseModel?) null
                );
                foreach (var course in responseCourse) mapCourse[course.UserItemId] = course;
            }
            if (isDecorateLesson)
            { // get list lesson
                List<Guid> listLessonIds = participants.Select(p => p.LessonId).Distinct().ToList();
                List<ViewLesson> lessons = await DbContext.ViewLessons.Where(l => listLessonIds.Contains(l.UserItemId)).ToListAsync();
                List<IResponseLessonModel> responseLesson = await DecorateResponseLessonModel(myUid, lessons);
                mapLesson = listLessonIds.ToDictionary(
                    id => id,
                    id => (IResponseLessonModel?)null
                );
                foreach (var lesson in responseLesson) mapLesson[lesson.UserItemId] = lesson;
            }

            List<ResponseCourseLessonModel> res = participants.Select(part =>
            {
                ResponseCourseLessonModel item = (ResponseCourseLessonModel) new ResponseCourseLessonModel().Copy(part);
                if (mapCourse.TryGetValue(item.CourseId, out IResponseCourseModel? value1))
                {
                    item.Course = value1;
                }
                if (mapLesson.TryGetValue(item.LessonId, out IResponseLessonModel? value2))
                {
                    item.Lesson = value2;
                }
                return item;
            }).ToList();
            return res;
        }

        public virtual async Task<List<IResponseLessonModel>> DecorateResponseLessonModel(Guid? myUid, List<ViewLesson> lessons)
        {
            List<IResponseLessonModel> res = lessons.Select(lesson =>
            {
                ResponseLessonModel les = new();
                les.Copy(lesson);
                return (IResponseLessonModel) les;
            }).ToList();
            await DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            return res;
        }

        public virtual async Task<List<IResponseQuestionModel>> DecorateResponseQuestionModel(Guid? myUid, List<ViewQuestion> questions)
        {
            List<IResponseQuestionModel> res = questions.Select(question =>
            {
                ResponseQuestionModel ques = new();
                ques.Copy(question);
                return (IResponseQuestionModel) ques;
            }).ToList();
            await DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            return res;
        }

        public virtual async Task<List<IResponseCourseModel>> DecorateResponseCourseModel(Guid? myUid, List<ViewCourse> courses)
        {
            List<ResponseCourseModel> res = courses.Select(course =>
            {
                ResponseCourseModel resCourse = new();
                resCourse.Copy(course);
                return resCourse;
            }).ToList();
            await DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            return res.OfType<IResponseCourseModel>().ToList();
        }

        public virtual async Task<List<ResponseUserCardModel>> DecorateResponseUserCardModel(Guid? myUid, List<ResponseUserCardModel> responseUserCardModels)
        {
            if (myUid != null)
            {
                List<Guid> userIds = responseUserCardModels.Select(card => card.UserId).ToList();
                Dictionary<Guid, UserRelationTypeDto> relationDict = 
                    await UserRelationRepository.GetDetailUserRelationType(myUid.Value, userIds);
                foreach (var card in responseUserCardModels)
                {
                    card.UserRelationType = relationDict[card.UserId].UserRelationType;
                    card.UserRelationId = relationDict[card.UserId].UserRelationId;
                }
            }
            return responseUserCardModels;
        }




        public virtual async Task<List<IResponseCommentModel>> DecorateResponseCommentModel(Guid? myUid, List<ViewComment> viewComments, bool isDecorateReplies = true)
        {
            List<ResponseCommentModel> res = viewComments.Select(comment =>
            {
                ResponseCommentModel resComment = new();
                resComment.Copy(comment);
                return resComment;
            }).ToList();
            await DecorateResponseUserItemModel(myUid, res.OfType<IResponseUserItemModel>().ToList());

            List<Guid> commentIds = viewComments.Select(com => com.UserItemId).ToList();

            // Lấy về ds tổng số replies
            Dictionary<Guid, int> totalReplies = await CommentRepository.GetTotalReplies(commentIds);
            foreach (ResponseCommentModel com in res)
            {
                com.TotalReplies = totalReplies[com.UserItemId];
            }
            return res.OfType<IResponseCommentModel>().ToList();
        }

        public virtual async Task<List<ResponseCourseRelationModel>> DecorateResponseCourseRelationModel(Guid? myUid, List<CourseRelation> relations, ECourseRelationType relationType, bool isDecorateUser = false, bool isDecorateCourse = false)
        {
            List<ResponseCourseRelationModel> res = relations.Select(relation =>
            {
                return (ResponseCourseRelationModel)new ResponseCourseRelationModel().Copy(relation);
            }).ToList();
            return await Task.FromResult(res);
        }

        public virtual async Task<List<ResponseStarModel>> DecorateResponseStarModel(List<Star> listStars, bool isDecorateUser = false, bool isDecorateItem = false)
        {
            Dictionary<Guid, ResponseUserCardModel>? mapUsers = null;
            Dictionary<Guid, IResponseUserItemModel?>? mapItems = null;
            if (isDecorateUser)
            {
                // Get Dictionary <userid, ViewUser -> ResponseUserCardModel>
                Dictionary<Guid, ViewUser?> listUsers = await UserRepository
                    .GetDetail(listStars.Select(star => star.UserId).ToArray());
                mapUsers = listUsers.ToDictionary(
                    item => item.Key,
                    item =>
                    {
                        ViewUser? user = item.Value;
                        ResponseUserCardModel resUser = new();
                        if (user != null) resUser.Copy(user);
                        return resUser;
                    }
                );
            }
            if (isDecorateItem)
            {
                // Get Dictionay <useritemid, UserItem -> ResponseUserItemModel>
                mapItems = await UserItemRepository
                    .GetExactlyResponseUserItemModel(listStars.Select(st => st.UserItemId).ToList());

            }
            List<ResponseStarModel> res = listStars.Select(star =>
            {
                ResponseStarModel resStar = new();
                resStar.Copy(star);
                // decorate list user
                if (isDecorateUser && mapUsers != null && mapUsers.TryGetValue(star.UserId, out ResponseUserCardModel? value1))
                {
                    resStar.User = value1;
                }

                // decorate list item
                if (isDecorateItem && mapItems != null && mapItems.TryGetValue(star.UserItemId, out IResponseUserItemModel? value2))
                {
                    resStar.Item = value2;
                }

                return resStar;
            }).ToList();
            return res;
        }

        public async Task<List<IResponsePostModel>> DecorateResponsePostModel(Guid? myUid, List<ViewPost> posts)
        {
            List<Guid> postIds = posts.Select(p => p.UserItemId).ToList();
            Dictionary<Guid, IResponseUserItemModel?> postDict = await PostRepository.GetExactlyResponseUserItemModel(postIds);
            List<IResponsePostModel> res = posts.Select(p =>
            {
                IResponseUserItemModel? rPost = postDict[p.UserItemId];
                if (rPost != null && rPost is IResponsePostModel value)
                {
                    return value;
                }
                if (p.PostType == EPostType.Lesson)
                {
                    return (ResponseLessonModel)new ResponseLessonModel().Copy(p);
                }
                return (ResponseQuestionModel)new ResponseQuestionModel().Copy(p);
            }).ToList();
            await DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            return res;
        }
    }
}
