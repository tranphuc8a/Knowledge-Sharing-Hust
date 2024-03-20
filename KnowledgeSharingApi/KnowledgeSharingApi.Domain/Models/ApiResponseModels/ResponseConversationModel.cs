using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseConversationModel : Conversation
    {
        public IEnumerable<ResponseParticipantModel> Participants { get; set; } = [];

        public IEnumerable<ResponseMessageModel> Messages { get; set; } = [];


        protected override ResponseConversationModel Init()
        {
            return new ResponseConversationModel();
        }
    }
}
