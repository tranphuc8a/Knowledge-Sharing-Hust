using KnowledgeSharingApi.Domains.Models.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewLesson")]
    public class ViewLesson : Lesson
    {
        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Cover { get; set; }


        public int? TotalStar { get; set; }

        public int? SumStar { get; set; }

        public double? AverageStar { get; set; }

        public int? TotalComment { get; set; }

        protected override ViewLesson Init()
        {
            return new ViewLesson();
        }
    }
}
