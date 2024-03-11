using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class Lesson : Post
    {
        public Guid LessonId { get; set; }
        public int EstimateTimeInMinutes { get; set; }

        //public override EPostType PostType { get => EPostType.Lesson; }
        protected override Post Init()
        {
            return new Lesson();
        }
    }
}
