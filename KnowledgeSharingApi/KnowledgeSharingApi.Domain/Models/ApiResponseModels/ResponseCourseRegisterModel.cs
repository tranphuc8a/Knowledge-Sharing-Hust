using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseCourseRegisterModel : ViewCourseRegister
    {
        public Guid? UserRelationId { get; set; } = null;

        public EUserRelationType? UserRelationType { get; set; } = null;
    }
}
