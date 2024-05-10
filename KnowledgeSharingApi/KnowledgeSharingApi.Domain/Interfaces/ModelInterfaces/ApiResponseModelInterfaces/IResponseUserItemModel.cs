using KnowledgeSharingApi.Domains.Annotations.Converters.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    [JsonConverter(typeof(ResponseUserItemConverter))]
    public interface IResponseUserItemModel
    {
        Guid UserItemId { get; set; }

        // Stars:
        double? AverageStars { get; set; }
        double? MyStars { get; set; }
        int TotalStars { get; set; }
    }
}
