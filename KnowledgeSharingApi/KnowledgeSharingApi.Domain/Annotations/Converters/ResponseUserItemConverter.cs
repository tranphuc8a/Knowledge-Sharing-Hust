using KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Converters
{
    [AttributeUsage(AttributeTargets.All)]
    public class ResponseUserItemConverterAttribute : Attribute
    {
    }

    public class ResponseUserItemConverter : JsonConverter<IResponseUserItemModel>
    {
        public override bool CanWrite => true;
        public override bool CanRead => false;

        public override IResponseUserItemModel ReadJson(JsonReader reader, Type objectType, IResponseUserItemModel? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException("ResponseUserItemConverter is only for writing JSON.");
        }

        public override void WriteJson(JsonWriter writer, IResponseUserItemModel? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            JObject obj = value switch
            {
                ResponseCommentModel comment => JObject.FromObject(comment, serializer),
                ResponseQuestionModel question => JObject.FromObject(question, serializer),
                ResponseLessonModel lesson => JObject.FromObject(lesson, serializer),
                ResponsePostModel post => JObject.FromObject(post, serializer),
                ResponseUserItemModel useritem => JObject.FromObject(useritem, serializer),
                _ => JObject.FromObject(value, serializer),
            };

            obj.WriteTo(writer);
        }
    }
}
