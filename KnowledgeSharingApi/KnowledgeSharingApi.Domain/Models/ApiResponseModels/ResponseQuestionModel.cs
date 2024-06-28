using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseQuestionModel : ViewQuestion, IResponseQuestionModel
    {
        public int NumberComments { get; set; }
        public List<ResponseCommentModel> TopComments { get; set; } = [];
        public bool IsMarked { get; set; }
        public double? AverageStars { get; set; }
        public double? MyStars { get; set; }
        public int TotalStars { get; set; }
        public List<string> Categories { get; set; } = [];
    }
}
