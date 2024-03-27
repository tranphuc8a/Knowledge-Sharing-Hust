using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class PaginationResponseModel<T>(int total, int limit, int offset, IEnumerable<T> result) where T : class
    {
        public PaginationResponseModel() : this(0, 0, 0, [])
        {
        }
        public int Total { get; set; } = total; 
        public int Limit { get; set; } = limit; 
        public int Offset { get; set; } = offset;
        public int Count { get => Results.Count<T>(); }

        public IEnumerable<T> Results { get; set; } = result;
    }
}
