using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("Lesson")]
    public class Lesson : Post
    {
        public int EstimateTimeInMinutes { get; set; }

        //public override EPostType PostType { get => EPostType.Lesson; }
        protected override Post Init()
        {
            return new Lesson();
        }
    }
}
