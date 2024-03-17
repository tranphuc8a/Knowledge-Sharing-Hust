using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("UserItem")]
    public class UserItem : Entity
    {
        public Guid UserItemId { get; set; }
        public Guid UserId { get; set; }
        public virtual EUserItemType UserItemType { get; set; }

        protected override UserItem Init()
        {
            return new UserItem();
        }
    }
}
