using KnowledgeSharingApi.Domains.Models.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewKnowledgeCategory")]
    public class ViewKnowledgeCategory : KnowledgeCategory
    {
        public string CategoryName { get; set; } = string.Empty;


        protected override ViewKnowledgeCategory Init()
        {
            return new ViewKnowledgeCategory();
        }
    }
}
