﻿using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Converters.ResponseModel;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    [JsonConverter(typeof(ResponseKnowledgeConverter))]
    public interface IResponseKnowledgeModel : IResponseUserItemModel
    {
        public int NumberComments { get; set; }

        public List<ResponseCommentModel> TopComments { get; set; }

        public List<string> Categories { get; set; }

        public bool IsMarked { get; set; }
    }
}
