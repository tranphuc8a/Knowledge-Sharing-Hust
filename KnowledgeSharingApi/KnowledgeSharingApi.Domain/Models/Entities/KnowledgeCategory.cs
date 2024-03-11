using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class KnowledgeCategory : Entity
    {
        public Guid KnowledgeCategoryId { get; set; }
        public Guid KnowledgeId { get; set; }
        public Guid CategoryId { get; set; }
        protected override KnowledgeCategory Init()
        {
            return new KnowledgeCategory();
        }
    }
}
