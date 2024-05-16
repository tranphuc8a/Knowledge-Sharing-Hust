using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class OrderDto
    {
        public string Field { get; set; } = string.Empty;

        public bool IsAscending { get; set; } = false;

        public OrderDto() { }

        public OrderDto(string field, bool isAscending)
        {
            Field = field;
            IsAscending = isAscending;
        }
    }
}
