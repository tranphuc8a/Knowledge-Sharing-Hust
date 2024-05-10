using KnowledgeSharingApi.Domains.Annotations.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    [JsonConverter(typeof(IResponseLessonModel))]
    public interface IResponseLessonModel : IResponsePostModel
    {
    }
}
