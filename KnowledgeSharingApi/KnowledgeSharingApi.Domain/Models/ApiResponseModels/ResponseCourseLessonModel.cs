using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseCourseLessonModel : CourseLesson
    {
        //[ResponseUserItemConverter]
        public IResponseLessonModel? Lesson { get; set; }

        //[ResponseUserItemConverter]
        public IResponseCourseModel? Course { get; set; }
    }
}
