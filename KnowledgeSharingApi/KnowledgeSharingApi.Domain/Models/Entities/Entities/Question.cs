using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class Question : Post
    {
        public Guid? CourseId { get; set; }
        public bool IsAccept { get; set; }


        //public override EPostType PostType { get => EPostType.Question; }
        protected override Post Init()
        {
            return new Question();
        }
    }
}
