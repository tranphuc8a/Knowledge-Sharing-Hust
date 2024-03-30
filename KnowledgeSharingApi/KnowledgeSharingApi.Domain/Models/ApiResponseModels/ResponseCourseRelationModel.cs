using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseCourseRelationModel : CourseRelation
    {

        public ResponseUserCardModel? User { get; set; }

        public ResponseCourseCardModel? Course { get; set; }

    }
}
