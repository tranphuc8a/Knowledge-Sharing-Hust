using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseUserCardModel : Entity
    {
        public Guid UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Cover { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public EUserRelationType UserRelationType { get; set; }

        protected override ResponseUserCardModel Init()
        {
            return new ResponseUserCardModel();
        }
    }
}
