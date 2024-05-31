using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class CourseRoleTypeDto
    {
        public ECourseRoleType? CourseRoleType { get; set; }

        public Guid? CourseRelationId { get; set; }
    }
}
