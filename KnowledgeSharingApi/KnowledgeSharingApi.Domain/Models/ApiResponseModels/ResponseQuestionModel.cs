using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseQuestionModel : ViewQuestion, IResponseKnowledgeModel
    {
        public int NumberComments { get; set; }
        public IEnumerable<IResponseCommentModel> TopComments { get; set; } = [];
        public bool IsMarked { get; set; }
        public double? AverageStars { get; set; }
        public double? MyStars { get; set; }
        public int TotalStars { get; set; }
    }
}
