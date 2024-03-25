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
    public class CourseRegister : Entity
    {
        [Key]
        public Guid CourseRegisterId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }


        protected override CourseRegister Init()
        {
            return new CourseRegister();
        }
    }
}
