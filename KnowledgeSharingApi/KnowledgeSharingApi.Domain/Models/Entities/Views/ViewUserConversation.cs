﻿using KnowledgeSharingApi.Domains.Models.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewUserConversation")]
    public class ViewUserConversation : UserConversation
    {
        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Avatar {  get; set; }

        public string? Cover { get; set; }



        protected override ViewUserConversation Init()
        {
            return new();
        }
    }
}
