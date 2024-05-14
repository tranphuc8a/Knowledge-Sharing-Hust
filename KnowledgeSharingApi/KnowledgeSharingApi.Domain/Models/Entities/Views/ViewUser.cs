using KnowledgeSharingApi.Domains.Models.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewUser")]
    public class ViewUser : Profile
    {
        public string Email {  get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public int? TotalFriend { get; set; }
        public int? TotalRequester { get; set; }
        public int? TotalRequestee { get; set; }
        public int? TotalFollower { get; set; }
        public int? TotalFolowee { get; set; }
        public int? TotalBlocker { get; set; }
        public int? TotalBlockee { get; set; }

        protected override ViewUser Init()
        {
            return new ViewUser();
        }
    }
}
