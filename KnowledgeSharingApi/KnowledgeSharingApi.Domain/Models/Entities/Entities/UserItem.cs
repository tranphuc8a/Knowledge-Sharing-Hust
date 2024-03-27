using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class UserItem : Entity, IUserItemView
    {
        [Key]
        public Guid UserItemId { get; set; }
        public Guid UserId { get; set; }
        public virtual EUserItemType UserItemType { get; set; }

        protected override UserItem Init()
        {
            return new UserItem();
        }
    }
}
