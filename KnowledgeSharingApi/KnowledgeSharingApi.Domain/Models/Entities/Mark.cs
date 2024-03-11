using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
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
