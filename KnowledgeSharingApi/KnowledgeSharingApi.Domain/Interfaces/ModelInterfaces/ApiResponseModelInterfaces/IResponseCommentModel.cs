using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    public interface IResponseCommentModel : IResponseUserItemModel
    {
        int TotalReplies { get; set; }
    }
}
