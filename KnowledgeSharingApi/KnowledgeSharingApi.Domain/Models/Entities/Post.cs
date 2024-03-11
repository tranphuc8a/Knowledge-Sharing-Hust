using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class Post : Knowledge
    {
        public Guid PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public virtual EPostType PostType { get; set; }

        //public override EKnowledgeType KnowledgeType { get => EKnowledgeType.Post; }
        protected override Post Init()
        {
            return new Post();
        }
    }
}
