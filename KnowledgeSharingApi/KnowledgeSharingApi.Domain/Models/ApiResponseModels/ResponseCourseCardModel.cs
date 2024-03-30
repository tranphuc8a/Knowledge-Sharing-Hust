using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseCourseCardModel : Entity
    {
        public Guid CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Abstract { get; set; } = string.Empty;

        public string? Thumbnail { get; set; }

        public ECourseRoleType Role { get; set; }

        protected override ResponseCourseCardModel Init()
        {
            return new ResponseCourseCardModel();
        }
    }
}
