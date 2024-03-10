using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Exceptions
{
    /// <summary>
    /// Exception cho các Validators ném ra
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class ValidatorException(string msg) : Exception(msg)
    {
        public ValidatorException() : this(String.Empty) { }
    }
}
