using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("Mark")]
    public class Mark : Entity
    {
        public Guid MarkId { get; set; }
        public Guid UserId { get; set; }
        public Guid KnowledgeId { get; set; }

        protected override Mark Init()
        {
            return new Mark();
        }
    }
}
