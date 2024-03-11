using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Exceptions
{
    public class DbContextNotMatchingException : Exception
    {
        const string message = "DbContext is not matching";
        public DbContextNotMatchingException() : base(message)
        {
            
        }
    }
}
