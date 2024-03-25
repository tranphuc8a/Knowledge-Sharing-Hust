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
    public class Knowledge : UserItem
    {
        public Guid KnowledgeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Abstract { get; set; }
        public string? Thumbnail { get; set; }
        public int Views { get; set; }
        public virtual EKnowledgeType KnowledgeType { get; set; }
        public EPrivacy Privacy { get; set; }
        //public override EUserItemType UserItemType { get => EUserItemType.Knowledge; }

        protected override Knowledge Init()
        {
            return new Knowledge();
        }
    }
}
