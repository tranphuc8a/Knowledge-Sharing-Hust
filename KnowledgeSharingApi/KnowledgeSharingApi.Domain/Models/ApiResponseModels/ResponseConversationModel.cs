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
        public List<ResponseParticipantModel> Participants { get; set; } = [];

        public List<ResponseMessageModel> Messages { get; set; } = [];


        protected override ResponseConversationModel Init()
        {
            return new ResponseConversationModel();
        }
    }
}
