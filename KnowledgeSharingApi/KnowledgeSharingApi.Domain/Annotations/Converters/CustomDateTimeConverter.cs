using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using KnowledgeSharingApi.Domains.Exceptions;

namespace KnowledgeSharingApi.Domains.Annotations.Converters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomDateTimeConverterAttribute : Attribute
    {
    }

    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        private const string DateFormat = "dd/MM/yyyy HH:mm:ss";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new ValidatorException(ViConstantResource.DATETIME_FORMAT);
            }

            string? str = reader.GetString();
            if (str == null || str == "")
            {
                return null;
            }

            if (DateTime.TryParseExact(str, DateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime value))
            {
                return value;
            }

            throw new ValidatorException(ViConstantResource.DATETIME_FORMAT);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Value.ToString(DateFormat));
        }
    }
}
