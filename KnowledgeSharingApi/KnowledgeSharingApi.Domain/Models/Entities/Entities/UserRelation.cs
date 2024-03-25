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
    public class UserRelation : Entity
    {
        [Key]
        public Guid UserRelationId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public EUserRelationType UserRelationType { get; set; }
        public DateTime Time { get; set; }

        protected override UserRelation Init()
        {
            return new UserRelation();
        }
    }
}
