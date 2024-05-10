using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseStarModel : Star
    {
        public ResponseUserCardModel? User { get; set; }

        //[JsonConverter(typeof(ResponseUserItemConverter))]
        public IResponseUserItemModel? Item { get; set; }
    }
}
