using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseLessonItemModel : ViewLesson
    {
        public double? Star { get; set; }

        public int NumberComments { get; set; }

        public IEnumerable<ViewComment> TopComments { get; set; } = [];
    }
}
