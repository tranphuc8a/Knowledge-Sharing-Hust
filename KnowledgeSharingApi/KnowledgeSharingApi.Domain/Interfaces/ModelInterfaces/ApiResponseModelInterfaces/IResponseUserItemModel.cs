using KnowledgeSharingApi.Domains.Annotations.Converters;
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
        // Stars:
        double? AverageStars { get; set; }
        double? MyStars { get; set; }
        int TotalStars { get; set; }
    }
}
