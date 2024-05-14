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
        ICourseRelationRepository courseRelationRepository,
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
        protected readonly ICourseRelationRepository CourseRelationRepository = courseRelationRepository;
        protected readonly IUserRepository UserRepository = userRepository;
        protected readonly IUserRelationRepository UserRelationRepository = userRelationRepository;
        protected readonly int NumberOfTopComments = 20;

        #region Decorate functional methods

        protected virtual async Task<List<IResponseUserItemModel>> DecorateResponseUserItemModel(Guid? myUid, List<IResponseUserItemModel> userItems)
        {
            // Attributes: myStar,
            List<Guid> uiid = userItems.Select(lesson => lesson.UserItemId).ToList();

            Dictionary<Guid, int?>? myStars = null;
            if (myUid != null)
            {
                // calculate myStar from myUid to all lessons
                myStars = await StarRepository.CalculateUserStars(myUid.Value, uiid);
            }

            if (myStars != null)
            {
                foreach (IResponseUserItemModel useritem in userItems)
                {
                    if (myStars.TryGetValue(useritem.UserItemId, out int? value))
                    {
                        useritem.MyStars = value;
                    }
                }
            }

            return userItems;
        }

        protected virtual async Task<List<IResponseKnowledgeModel>> DecorateResponseKnowledgeModel
            (Guid? myUid, List<IResponseKnowledgeModel> knowledges)
        {
            await DecorateResponseUserItemModel(myUid, knowledges.OfType<IResponseUserItemModel>().ToList());

            // Attributes: categories, ismark
            List<Guid> knowledgeIds = knowledges.Select(lesson => lesson.UserItemId).ToList();

            Dictionary<Guid, bool>? isMarks = null;
            Task<Dictionary<Guid, bool>>? promise2 = null;
            if (myUid != null)
            {
                // is Mark
                promise2 = MarkRepository.GetUserMarkListKnowledge(myUid.Value, knowledgeIds);
            }
            if (promise2 != null) isMarks = await promise2;
            // Number comments & Top comments
            // Dictionary<Guid, PaginationResponseModel<ViewComment>?> topComments =
            //    await CommentRepository.GetListCommentsOfKnowledge(knowledgeIds, NumberOfTopComments);

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
                //knowledge.NumberComments = topComments[knowledge.UserItemId]?.Total ?? 0;
                //knowledge.TopComments = topComments[knowledge.UserItemId] != null ?
                //    topComments[knowledge.UserItemId]!.Results.Select(viewComment =>
                //    {
                //        ResponseCommentModel resComment = new();
                //        resComment.Copy(viewComment);
                //        return resComment;
                //    }).ToList() : [];
                if (lsCategories.TryGetValue(knowledge.UserItemId, out List<Category>? value))
                {
                    knowledge.Categories = value ?? [];
                }
                if (isMarks != null && isMarks.TryGetValue(knowledge.UserItemId, out bool value2))
                {
                    knowledge.IsMarked = value2;
                };
            }

            return knowledges;
        }

        protected override DbSet<UserItem> GetDbSet()
        {
            return DbContext.UserItems;
        }

        #endregion

        protected virtual async Task<Dictionary<Guid, IResponseCourseModel?>> GetMapCourse(Guid? myUid, List<Guid> courseIds)
        {
            Dictionary<Guid, IResponseCourseModel?> mapCourse = [];
            List<ViewCourse> courses = await DbContext.ViewCourses.Where(c => courseIds.Contains(c.UserItemId)).ToListAsync();
            List<IResponseCourseModel> responseCourse = await DecorateResponseCourseModel(myUid, courses);
            mapCourse = courseIds.ToDictionary(
                id => id,
                id => (IResponseCourseModel?)null
            );
            foreach (var course in responseCourse)
            {
                mapCourse[course.UserItemId] = course;
            }
            return mapCourse;
        }
        protected virtual async Task<Dictionary<Guid, IResponseLessonModel?>> GetMapLesson(Guid? myUid, List<Guid> lessonids)
        {
            Dictionary<Guid, IResponseLessonModel?> mapLesson = [];
            List<ViewLesson> lessons = await DbContext.ViewLessons.Where(l => lessonids.Contains(l.UserItemId)).ToListAsync();
            List<IResponseLessonModel> responseLesson = await DecorateResponseLessonModel(myUid, lessons);
            mapLesson = lessonids.ToDictionary(
                id => id,
                id => (IResponseLessonModel?)null
            );
            foreach (var lesson in responseLesson) mapLesson[lesson.UserItemId] = lesson;
            return mapLesson;
        }
        public virtual async Task<List<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid, List<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false)
        {
            // Atrributes: Course, Lesson
            Dictionary<Guid, IResponseCourseModel?> mapCourse = [];
            Dictionary<Guid, IResponseLessonModel?> mapLesson = [];
            Task<Dictionary<Guid, IResponseCourseModel?>> mapCoursePromise = Task.FromResult(mapCourse);
            Task<Dictionary<Guid, IResponseLessonModel?>> mapLessonPromise = Task.FromResult(mapLesson);

            if (isDecorateCourse)
            { // Get list course
                List<Guid> listCourseIds = participants.Select(p => p.CourseId).Distinct().ToList();
                mapCoursePromise = GetMapCourse(myUid, listCourseIds);
            }
            mapCourse = await mapCoursePromise;
            if (isDecorateLesson)
            { // get list lesson
                List<Guid> listLessonIds = participants.Select(p => p.LessonId).Distinct().ToList();
                mapLessonPromise = GetMapLesson(myUid, listLessonIds);
            }
            mapLesson = await mapLessonPromise;
            //await Task.WhenAll([mapCoursePromise, mapLessonPromise]);
            //mapCourse = mapCoursePromise.Result;
            //mapLesson = mapLessonPromise.Result;

            List<ResponseCourseLessonModel> res = participants.Select(part =>
            {
                ResponseCourseLessonModel item = (ResponseCourseLessonModel)new ResponseCourseLessonModel().Copy(part);
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
                return (IResponseLessonModel)les;
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
                return (IResponseQuestionModel)ques;
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
            
            // Decorate CourseRoleType
            if (myUid != null)
            {
                Dictionary<Guid, CourseRoleTypeDto> mapCourseRoleType = await CourseRelationRepository
                    .GetCourseRoleType(myUid.Value, courses.Select(c => c.UserItemId).ToList());
                foreach (IResponseCourseModel course in res)
                {
                    if (mapCourseRoleType.TryGetValue(course.UserItemId, out CourseRoleTypeDto? role))
                    {
                        course.CourseRoleType = role.CourseRoleType;
                        course.CourseRelationId = role.CourseRelationId;
                    }
                }
            }

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
                    if (relationDict.TryGetValue(card.UserId, out UserRelationTypeDto? value))
                    {
                        card.UserRelationType = value.UserRelationType;
                        card.UserRelationId = value.UserRelationId;
                    }
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

            //List<Guid> commentIds = viewComments.Select(com => com.UserItemId).ToList();

            // Lấy về ds tổng số replies
            // Dictionary<Guid, int> totalReplies = await CommentRepository.GetTotalReplies(commentIds);
            //foreach (ResponseCommentModel com in res)
            //{
            //    com.TotalReplies = totalReplies[com.UserItemId];
            //}
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

        protected virtual async Task<Dictionary<Guid, ResponseUserCardModel>> GetMapUserCard(List<Guid> userIds)
        {
            Dictionary<Guid, ResponseUserCardModel> mapUsers = [];
            Dictionary<Guid, ViewUser?> listUsers = await UserRepository
                    .GetDetail(userIds.ToArray());
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
            return mapUsers;
        }
        public virtual async Task<List<ResponseStarModel>> DecorateResponseStarModel(List<Star> listStars, bool isDecorateUser = false, bool isDecorateItem = false)
        {
            Dictionary<Guid, ResponseUserCardModel> mapUsers = [];
            Dictionary<Guid, IResponseUserItemModel?> mapItems = [];
            Task<Dictionary<Guid, ResponseUserCardModel>> mapUserPromise = Task.FromResult(mapUsers);
            Task<Dictionary<Guid, IResponseUserItemModel?>> mapUserItemPromise = Task.FromResult(mapItems);

            if (isDecorateUser)
            {
                // Get Dictionary <userid, ViewUser -> ResponseUserCardModel>
                mapUserPromise = GetMapUserCard(listStars.Select(st => st.UserId).ToList());
            }
            mapUsers = await mapUserPromise;
            if (isDecorateItem)
            {
                // Get Dictionay <useritemid, UserItem -> ResponseUserItemModel>
                mapUserItemPromise = UserItemRepository
                    .GetExactlyResponseUserItemModel(listStars.Select(st => st.UserItemId).ToList());
            }
            mapItems = await mapUserItemPromise;
            //await Task.WhenAll([mapUserPromise, mapUserItemPromise]);
            //mapUsers = mapUserPromise.Result;
            //mapItems = mapUserItemPromise.Result;

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
            var postDictPromise = PostRepository.GetExactlyResponseUserItemModel(postIds);
            Dictionary<Guid, IResponseUserItemModel?> postDict = await postDictPromise;

            List<IResponsePostModel> res = posts.Select<ViewPost, IResponsePostModel>(p =>
            {
                if (p.PostType == EPostType.Lesson)
                {
                    return (ResponseLessonModel)new ResponseLessonModel().Copy(p);
                }
                return (ResponseQuestionModel)new ResponseQuestionModel().Copy(p);
            }).ToList();

            //await Task.WhenAll([postDictPromise, decoratePromise]);
            //Dictionary<Guid, IResponseUserItemModel?> postDict = postDictPromise.Result;
            foreach (IResponsePostModel item in res)
            {
                if (postDict.TryGetValue(item.UserItemId, out IResponseUserItemModel? value) && value != null)
                {
                    if (item is ResponseLessonModel resLes)
                    {
                        resLes.Copy(value);
                    }
                    else if (item is ResponseQuestionModel resQues)
                    {
                        resQues.Copy(value);
                    }
                }
            }

            var decoratePromise = DecorateResponseKnowledgeModel(myUid, res.OfType<IResponseKnowledgeModel>().ToList());
            await decoratePromise;
            return res;
        }
    }
}
