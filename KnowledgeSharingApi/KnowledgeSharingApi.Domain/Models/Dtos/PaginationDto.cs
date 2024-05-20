using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class PaginationDto
    {
        public int? Limit { get; set; }
        
        public int? Offset { get; set; }
        
        public List<OrderDto>? Orders { get; set; }

        public List<FilterDto>? Filters { get; set; }

        public PaginationDto() { }

        public PaginationDto(int? limit, int? offset, List<OrderDto>? orders, List<FilterDto>? filters)
        {
            Limit = limit;
            Offset = offset;
            Orders = orders;
            Filters = filters;
        }
    }
}
