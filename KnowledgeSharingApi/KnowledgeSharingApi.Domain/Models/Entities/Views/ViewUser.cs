using KnowledgeSharingApi.Domains.Models.Entities.Tables;
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
        protected override ViewUser Init()
        {
            return new ViewUser();
        }
    }
}
