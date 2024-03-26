﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseFriendCardModel
    {
        public Guid FriendId {get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public bool IsActive { get; set; }

        public DateTime Time { get; set; }
    }
}