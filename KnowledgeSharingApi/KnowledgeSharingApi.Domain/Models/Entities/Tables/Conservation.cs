using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("Conservation")]
    public class Conservation : Entity
    {
        public Guid ConservationId { get; set; } = Guid.Empty;
        public string ConservationName { get; set; } = string.Empty;
        public string? Thumbnail { get; set; }
        protected override Conservation Init()
        {
            return new Conservation();
        }
    }
}
