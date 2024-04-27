using KnowledgeSharingApi.Domains.Models.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewCoursePayment")]
    public class ViewCoursePayment : CoursePayment
    {
        public string FullName { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Cover { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Thumbnail { get; set; }

        protected override ViewCoursePayment Init()
        {
            return new ViewCoursePayment();
        }
    }
}
