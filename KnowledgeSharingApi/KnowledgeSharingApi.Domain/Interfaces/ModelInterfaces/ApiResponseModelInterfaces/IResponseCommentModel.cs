using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Converters.ResponseModel;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    [JsonConverter(typeof(ResponseCommentConverter))]
    public interface IResponseCommentModel : IResponseUserItemModel
    {
        int TotalReplies { get; set; }

        ResponseCommentModel? Reply { get; set; }
    }
}
