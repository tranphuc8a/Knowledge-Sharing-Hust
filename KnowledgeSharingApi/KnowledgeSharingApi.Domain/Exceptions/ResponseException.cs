using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Exceptions
{
    /// <summary>
    /// Exception cho các Response ném ra
    /// </summary>
    /// Created: PhucTV (10/3/24)
    /// MOdified: None
    public class ResponseException : Exception
    {
        public int StatusCode { get; set; }
        public string? UserMessage { get; set; }
        public string? DevMessage { get; set; }
        public object? Body { get; set; }

        public override string Message
        {
            get
            {
                return UserMessage ?? DevMessage ?? String.Empty;
            }
        }
    }
}

