using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseCourseModel : ViewCourse, IResponseCourseModel
    {
        public int NumberComments { get; set; }
        public List<ResponseCommentModel> TopComments { get; set; } = [];
        public bool IsMarked { get; set; }
        public double? AverageStars { get; set; }
        public double? MyStars { get; set; }
        public int TotalStars { get; set; }
        public ECourseRoleType? Role { get; set; }
        public List<string> Categories { get; set; } = [];
        public ECourseRoleType? CourseRoleType { get; set; }
        public Guid? CourseRelationId { get; set; }
    }
}
