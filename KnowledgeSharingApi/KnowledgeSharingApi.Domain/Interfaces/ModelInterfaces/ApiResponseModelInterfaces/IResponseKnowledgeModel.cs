using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    public interface IResponseKnowledgeModel : IResponseUserItemModel
    {
        public int NumberComments { get; set; }

        public IEnumerable<IResponseCommentModel> TopComments { get; set; }

        public bool IsMarked { get; set; }
    }
}
