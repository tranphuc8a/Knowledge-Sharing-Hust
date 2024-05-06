using KnowledgeSharingApi.Domains.Annotations.Converters;
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
        public ResponseLessonModel? Lesson { get; set; }

        //[ResponseUserItemConverter]
        public ResponseCourseModel? Course { get; set; }
    }
}
