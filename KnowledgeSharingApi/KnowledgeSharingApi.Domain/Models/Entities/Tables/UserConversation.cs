﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("UserConversation")]
    public class UserConversation : Entity
    {
        public Guid UserConversationId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid ConversationId { get; set; } = Guid.Empty;
        public string? Nickname { get; set; }
        public DateTime Time { get; set; }
        public DateTime LastReadTime { get; set; }
        public DateTime LastDeleteTime { get; set; }
        protected override UserConversation Init()
        {
            return new UserConversation();
        }
    }
}
