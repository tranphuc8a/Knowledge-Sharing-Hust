using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class UserRelationTypeDto
    {
        public EUserRelationType? UserRelationType { get; set; }

        public Guid? UserRelationId { get; set; }
    }
}
