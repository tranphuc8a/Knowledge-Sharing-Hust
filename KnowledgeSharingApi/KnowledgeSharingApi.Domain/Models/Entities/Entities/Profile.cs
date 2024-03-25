using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class Profile : Entity
    {
        [Key]
        public Guid ProfileId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Cover { get; set; }
        public string? Nickname { get; set; }
        public string? Bio { get; set; }
        public EGender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ContactEmail { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public string? SocialLink { get; set; }
        public string? School { get; set; }
        public string? Profession { get; set; }
        public double? Cpa { get; set; }
        public string? Grade { get; set; }
        public string? Class { get; set; }
        public string? Job { get; set; }

        protected override Profile Init()
        {
            return new Profile();
        }
    }
}
