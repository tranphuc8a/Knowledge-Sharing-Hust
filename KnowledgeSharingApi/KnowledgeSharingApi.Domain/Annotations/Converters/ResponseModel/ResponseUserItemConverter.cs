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
    public class ResponseUserItemConverterAttribute : JsonConverterAttribute
    {
        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            if (typeof(IResponseUserItemModel).IsAssignableFrom(typeToConvert))
            {
                return new ResponseUserItemConverter();
            }

            throw new InvalidOperationException($"The converter {nameof(ResponseUserItemConverter)} cannot handle {typeToConvert}.");
        }
    }

    public class ResponseUserItemConverter : JsonConverter<IResponseUserItemModel>
    {
        public override IResponseUserItemModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("ResponseUserItemConverter is only for writing JSON.");
        }

        public override void Write(Utf8JsonWriter writer, IResponseUserItemModel value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

    }
}
