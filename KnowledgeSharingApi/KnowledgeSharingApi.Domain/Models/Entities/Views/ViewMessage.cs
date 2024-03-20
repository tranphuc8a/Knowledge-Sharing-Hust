﻿using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewMessage")]
    public class ViewMessage : Message
    {
        public Guid UserId { get; set; }

        public Guid ConversationId { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Thumbnail { get; set; } = null;



        protected override ViewMessage Init()
        {
            return new ViewMessage();
        }
    }
}