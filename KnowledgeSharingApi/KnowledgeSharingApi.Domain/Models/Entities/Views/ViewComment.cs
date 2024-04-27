using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewComment")]
    public class ViewComment : Comment
    {
        public string FullName { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Cover { get; set; }

        protected override ViewComment Init() {
            return new ViewComment();
        }
    }

    
}
