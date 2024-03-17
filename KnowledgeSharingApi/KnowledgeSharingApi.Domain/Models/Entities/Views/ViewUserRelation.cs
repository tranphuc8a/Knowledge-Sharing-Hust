using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewUserRelation")]
    public class ViewUserRelation : UserRelation
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderUsername { get; set; } = string.Empty;
        public string SenderRole { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public string ReceiverUsername { get; set; } = string.Empty;
        public string ReceiverRole { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;

        protected override ViewUserRelation Init()
        {
            return new ViewUserRelation();
        }
    }
}
