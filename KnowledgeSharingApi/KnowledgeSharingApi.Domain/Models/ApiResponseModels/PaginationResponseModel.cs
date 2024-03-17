using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class PaginationResponseModel<T> where T : class
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Count { get => Results.Count<T>(); }

        public IEnumerable<T> Results { get; set; } = [];
    }
}
