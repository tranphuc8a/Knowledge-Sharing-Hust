using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("KnowledgeCategory")]
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
