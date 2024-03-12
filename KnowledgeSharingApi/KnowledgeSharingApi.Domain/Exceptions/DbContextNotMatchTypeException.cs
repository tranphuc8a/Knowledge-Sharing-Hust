using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Exceptions
{
    public class DbContextNotMatchTypeException(string? message = null) : NotMatchTypeException(message ?? UserMessage)
    {
        const string UserMessage = "DbContext is not matching";
    }
}
