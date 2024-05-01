using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class Image : Entity
    {
        public Guid ImageId { get; set; } = Guid.Empty;

        public Guid UserId { get; set; } = Guid.Empty;

        public string Url { get; set; } = string.Empty;


        protected override Image Init()
        {
            return new Image();
        }
    }
}
