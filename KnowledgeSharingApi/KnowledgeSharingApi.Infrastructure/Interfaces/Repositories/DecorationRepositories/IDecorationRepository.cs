using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories
{
    public interface IDecorationRepository
    {
        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseLessonModel từ viewLesson
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="lessons"> Danh sách các view lessons </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<List<ResponseLessonModel>> DecorateResponseLessonModel(Guid? myUid, List<ViewLesson> lessons);

        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseQuestionModel từ viewQuestion
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="questions"> Danh sách các view questions </param>
        /// <returns></returns>
        /// Created: PhucTV (3/5/24)
        /// Modified: None
        Task<List<ResponseQuestionModel>> DecorateResponseQuestionModel(Guid? myUid, List<ViewQuestion> questions);

        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseCourseModel từ viewCourse
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="courses"> Danh sách các viewCourses </param>
        /// <returns></returns>
        /// Created: PhucTV (3/5/24)
        /// Modified: None
        Task<List<ResponseCourseModel>> DecorateResponseCourseModel(Guid? myUid, List<ViewCourse> courses);

        /// <summary>
        /// Bổ sung thêm thông tin cho ResponseCourseLessonModel từ CourseLesson
        /// </summary>
        /// <param name="myUid"> id của user thực hiện </param>
        /// <param name="participants"> Danh sách bài giảng trong khóa học </param>
        /// <param name="isDecorateCourse"> true - có, false - không decorate Lesson </param>
        /// <param name="isDecorateLesson"> true - có, false - không decorate Course </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        Task<List<ResponseCourseLessonModel>> DecorateResponseCourseLessonModel(Guid? myUid,
            List<CourseLesson> participants, bool isDecorateLesson = false, bool isDecorateCourse = false);

        /// <summary>
        /// Them thong tin ve quan he ban be 
        /// </summary>
        /// <param name="myUid"> id chu the </param>
        /// <param name="responseUserCardModels"> Danh sach userCardModels can decorate </param>
        /// <returns></returns>
        /// Created: PhucTV (7/5/24)
        /// Modified: None
        Task<List<ResponseUserCardModel>> DecorateResponseUserCardModel(Guid? myUid, List<ResponseUserCardModel> responseUserCardModels);


        /// <summary>
        /// Thêm các giá trị bổ sung cho mỗi comment của danh sách comment:
        /// + trung bình số sao, tổng số sao, số sao của user hiện tại nếu có
        /// </summary>
        /// <param name="myUid"> id của người dùng hiện tại </param>
        /// <param name="viewComments"> Danh sách comment cần decorate </param
        /// <returns></returns>
        /// Created: PhucTV (26/3/24)
        /// Modified: None
        Task<List<ResponseCommentModel>> DecorateResponseCommentModel(Guid? myUid, List<ViewComment> viewComments, bool isDecorateReplies = true);


        /// <summary>
        /// Trang tri va bo sung them thong tin cho ResponseCourseRelationModel tu CourseRelation
        /// </summary>
        /// <param name="myUid"> id cua user thuc hien </param>
        /// <param name="relations"> Danh sach relation can trang tri </param>
        /// <param name="relationType"> Loai relation </param>
        /// <param name="isDecorateUser"> Co trang tri user khong </param>
        /// <param name="isDecorateCourse"> Co trang tri course khong </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<List<ResponseCourseRelationModel>> DecorateResponseCourseRelationModel(
            Guid? myUid,
            List<CourseRelation> relations,
            ECourseRelationType relationType,
            bool isDecorateUser = false,
            bool isDecorateCourse = false);

        /// <summary>
        /// Trang trí thêm thông tin cho ResponseStarModel
        /// </summary>
        /// <param name="listStars"> Danh sách star cần trang trí </param>
        /// <param name="isDecorateUser"> Có decorate user không </param>
        /// <param name="isDecorateItem"> Có decorate item không </param>
        /// <returns></returns>
        /// Created: PhucTV (7/5/24)
        /// Modified: None
        Task<List<ResponseStarModel>> DecorateResponseStarModel
            (List<Star> listStars, bool isDecorateUser = false, bool isDecorateItem = false);
    }
}
