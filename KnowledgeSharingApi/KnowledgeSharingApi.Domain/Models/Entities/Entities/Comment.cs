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
    public class Comment : UserItem
    {
        public Guid KnowledgeId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid? ReplyId { get; set; }
        //public override EUserItemType UserItemType
        //{
        //    get => EUserItemType.Comment;
        //}
        protected override Comment Init()
        {
            return new Comment();
        }
    }
}
