using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseMessageModel : ViewMessage
    {
        public ResponseMessageModel? ReplyMessage { get; set; } = null;


        protected override ResponseMessageModel Init()
        {
            return new ResponseMessageModel();
        }
    }
}
