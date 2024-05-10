using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Converters.ResponseModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ResponseLessonConverterAttribute : JsonConverterAttribute
    {
        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            if (typeof(IResponseLessonModel).IsAssignableFrom(typeToConvert))
            {
                return new ResponseLessonConverter();
            }

            throw new InvalidOperationException($"The converter {nameof(ResponseLessonConverter)} cannot handle {typeToConvert}.");
        }
    }

    public class ResponseLessonConverter : JsonConverter<IResponseLessonModel>
    {
        public override IResponseLessonModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("ResponseLessonConverter is only for writing JSON.");
        }

        public override void Write(Utf8JsonWriter writer, IResponseLessonModel value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

    }
}
