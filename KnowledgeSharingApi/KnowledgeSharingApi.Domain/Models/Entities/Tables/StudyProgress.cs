using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("StudyProgress")]
    public class StudyProgress : Entity
    {
        public Guid StudyProgressId { get; set; }
        public Guid UserId { get; set; }
        public Guid KnowledgeId { get; set; }
        public double Progress { get; set; } = 0;
        protected override StudyProgress Init()
        {
            return new StudyProgress();
        }
    }
}
