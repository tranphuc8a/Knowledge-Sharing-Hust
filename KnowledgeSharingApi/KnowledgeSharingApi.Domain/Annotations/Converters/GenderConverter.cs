using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Annotations.Converters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GenderConverterAttribute : Attribute
    {
    }

    public class GenderConverter : JsonConverter<EGender?>
    {
        public override EGender? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            // Chấp nhận string "0", "1", "2"
            if (reader.TokenType == JsonTokenType.String)
            {
                string? str = reader.GetString();
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }

                if (int.TryParse(str, out int value))
                {
                    if (Enum.IsDefined(typeof(EGender), value))
                    {
                        return (EGender)value;
                    }
                }
            }
            // Chấp nhận int: 0, 1, 2
            else if (reader.TokenType == JsonTokenType.Number)
            {
                int value = reader.GetInt32();
                if (Enum.IsDefined(typeof(EGender), value))
                {
                    return (EGender)value;
                }
            }

            // Xử lý trường hợp không hợp lệ
            throw new JsonException(ViConstantResource.GENDER_FORMAT);
        }

        public override void Write(Utf8JsonWriter writer, EGender? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }

            int intValue = (int)value.Value;
            writer.WriteStringValue(intValue.ToString());
        }
    }
}
