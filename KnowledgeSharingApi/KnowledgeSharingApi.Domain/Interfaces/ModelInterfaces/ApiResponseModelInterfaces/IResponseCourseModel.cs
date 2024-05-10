using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Converters.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    [JsonConverter(typeof(ResponseCourseConverter))]
    public interface IResponseCourseModel : IResponseKnowledgeModel
    {
    }
}
