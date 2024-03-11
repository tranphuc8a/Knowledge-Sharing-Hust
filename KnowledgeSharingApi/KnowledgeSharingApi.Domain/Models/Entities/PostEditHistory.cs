using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class PostEditHistory : Entity
    {
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
