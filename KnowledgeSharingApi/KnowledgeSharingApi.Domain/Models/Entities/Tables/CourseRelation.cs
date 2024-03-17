using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("CourseRelation")]
    public class CourseRelation : Entity
    {
        public Guid CourseRelationId { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
        public Guid CourseId { get; set; }
        public ECourseRelationType CourseRelationType { get; set; }


        protected override CourseRelation Init()
        {
            return new CourseRelation();
        }
    }
}
