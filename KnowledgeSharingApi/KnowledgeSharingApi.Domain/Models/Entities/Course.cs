using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class Course : Knowledge
    {
        public Guid CourseId { get; set; }
        public string Introduction {  get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public int EstimateTimeInMinutes { get; set; }
        public bool IsFee { get; set; }
        //public override EKnowledgeType KnowledgeType { get => EKnowledgeType.Course; }

        protected override Course Init()
        {
            return new Course();
        }
    }
}
