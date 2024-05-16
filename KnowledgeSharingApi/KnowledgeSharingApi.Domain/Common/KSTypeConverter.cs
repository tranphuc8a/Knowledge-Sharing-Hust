using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Common
{
    public static class KSTypeConverter
    {
        public static bool TryParseValue<Type>(string value, out object? result)
        {
            return TryParseValue(value, typeof(Type), out result);
        }
        public static bool TryParseValue(string value, Type targetType, out object? result)
        {
            // Xử lý cho DateTime, Guid, Enum, Boolean
            if (targetType == typeof(DateTime))
                return TryParseDateTimeValue(value, out result);
            else if (targetType == typeof(Guid))
                return TryParseGuidValue(value, out result);
            else if (targetType.IsEnum)
                return TryParseEnumValue(value, targetType, out result);
            else if (targetType == typeof(bool))
                return TryParseBoolValue(value, out result);

            // Normal Converter
            return TryParseDefaultValue(value, targetType, out result);
        }


        public static bool TryParseDateTimeValue(string value, out object? result)
        {
            bool success = DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeResult);
            result = dateTimeResult;
            return success;
        }

        public static bool TryParseGuidValue(string value, out object? result)
        {
            bool success = Guid.TryParse(value, out Guid guidResult);
            result = guidResult;
            return success;
        }

        public static bool TryParseEnumValue(string value, Type targetType, out object? result)
        {
            try
            {
                // Attempt to parse the value as the name of the enum
                result = Enum.Parse(targetType, value, ignoreCase: true);
                return true;
            }
            catch
            {
                // If parsing as a name fails, try to parse as the underlying numeric value
                if (int.TryParse(value, out int intValue))
                {
                    result = Enum.ToObject(targetType, intValue);
                    return true;
                }
                result = null;
                return false;
            }
        }

        public static bool TryParseDefaultValue(string value, Type targetType, out object? result)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(targetType);
            try
            {
                if (converter != null && converter.IsValid(value))
                {
                    result = converter.ConvertFromInvariantString(value);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryParseBoolValue(string value, out object? result)
        {
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase) || value == "1")
            {
                result = true;
                return true;
            }
            else if (value.Equals("false", StringComparison.OrdinalIgnoreCase) || value == "0")
            {
                result = false;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
