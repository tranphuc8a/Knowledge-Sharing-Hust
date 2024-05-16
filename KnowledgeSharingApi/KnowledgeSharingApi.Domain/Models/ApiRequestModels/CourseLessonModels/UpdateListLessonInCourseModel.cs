using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels
{
    public class UpdateListLessonInCourseItemModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid? LessonId { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.CONTENT_EMPTY)]
        public string? LessonTitle { get; set; }
    }

    public class UpdateListLessonInCourseModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid? CourseId { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.CONTENT_EMPTY)]
        public List<UpdateListLessonInCourseItemModel>? ListLessonModel { get; set; }
    }
}
