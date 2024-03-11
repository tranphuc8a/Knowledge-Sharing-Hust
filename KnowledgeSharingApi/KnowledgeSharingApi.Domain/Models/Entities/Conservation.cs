using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class Conservation : Entity
    {
        public Guid ConservationId { get; set; } = Guid.Empty;
        public string ConservationName { get; set; } = String.Empty;
        protected override Conservation Init()
        {
            return new Conservation();
        }
    }
}
