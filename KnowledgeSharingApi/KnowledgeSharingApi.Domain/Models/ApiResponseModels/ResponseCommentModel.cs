using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseCommentModel : ViewComment, IResponseCommentModel
    {
        // public int Temp { get; set; } = 0;
        public double? AverageStars { get; set; }
        public double? MyStars { get; set; }
        public int TotalStars { get; set; }
        public int TotalReplies { get; set; }
    }
}
