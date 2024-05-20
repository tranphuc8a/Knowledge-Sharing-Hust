using KnowledgeSharingApi.Domains.Annotations.Converters.ResponseModel;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    //[JsonConverter(typeof(ResponseUserItemConverter))]
    public class ResponseLessonModel : ViewLesson, IResponseLessonModel
    {
        public double? AverageStars { get; set; }
        public double? MyStars { get; set; }
        public int TotalStars { get; set; }

        public int NumberComments { get; set; }
        public List<ResponseCommentModel> TopComments { get; set; } = [];
        public bool IsMarked { get; set; }
        public List<Category> Categories { get; set; } = [];
    }
}
