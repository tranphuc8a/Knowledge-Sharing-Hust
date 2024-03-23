using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels
{
    public class UpdateKnowledgeModel : UpdateUserItemModel
    {
        public string? Title { get; set; }

        public string? Abstract { get; set; }

        public IFormFile? Thumbnail { get; set; }

        public IEnumerable<string>? Categories { get; set; }
    }
}
