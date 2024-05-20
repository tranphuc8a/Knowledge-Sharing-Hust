using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class FilterDto
    {
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Operation { get; set; } = FilterOperations.Equal;

        public FilterDto() { }

        public FilterDto(string field, string value, string operation)
        {
            Field = field;
            Value = value;
            Operation = operation;
        }
    }
}
