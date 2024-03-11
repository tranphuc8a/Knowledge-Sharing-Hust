using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class CourseLesson : Entity
    {
        public Guid CourseLessonId { get; set; }
        public Guid CourseId { get; set; }
        public Guid LessonId { get; set; }
        public int Offset { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        protected override CourseLesson Init()
        {
            return new CourseLesson();
        }
    }
}
