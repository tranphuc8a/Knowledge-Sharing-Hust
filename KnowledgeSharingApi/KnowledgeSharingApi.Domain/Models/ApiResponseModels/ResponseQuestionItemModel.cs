using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseQuestionItemModel : ViewQuestion
    {
        public double? Star { get; set; }

        public int NumberComments { get; set; }

        public IEnumerable<ViewComment> TopComments { get; set; } = [];

        public bool IsMarked { get; set; }

        public int? MyStar { get; set; }
    }
}
