using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class Question : Post
    {
        public Guid QuestionId { get; set; }
        public Guid? CourseId { get; set; }
        public bool IsAccept { get; set; }


        //public override EPostType PostType { get => EPostType.Question; }
        protected override Post Init()
        {
            return new Question();
        }
    }
}
