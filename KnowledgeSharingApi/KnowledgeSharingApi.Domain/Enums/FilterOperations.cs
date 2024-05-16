using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Enums
{
    public static class FilterOperations
    {
        public const string Equal = "eq";
        public const string NotEqual = "ne";
        public const string GreaterThan = "gt";
        public const string GreaterThanOrEqual = "gte";
        public const string LessThan = "lt";
        public const string LessThanOrEqual = "lte";
        public const string Contain = "ctn";
        public const string Like = "like";
    }
}
