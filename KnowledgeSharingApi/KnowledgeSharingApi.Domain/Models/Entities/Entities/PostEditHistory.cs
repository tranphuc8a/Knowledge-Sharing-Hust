using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class PostEditHistory : Entity
    {
        [Key]
        public Guid PostEditHistoryId { get; set; }
        public Guid PostId { get; set; }
        public string? Title { get; set; }
        public string? Abstract { get; set; }
        public string? Thumbnail { get; set; }
        public string? Content { get; set; }

        protected override PostEditHistory Init()
        {
            return new PostEditHistory();
        }
    }
}
