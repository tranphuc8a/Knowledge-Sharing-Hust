using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels
{
    public class CreateKnowledgeModel : CreateUserItemModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.TITLE_EMPTY)]
        public string? Title { get; set; }

        public string? Abstract { get; set; }

        public IFormFile? Thumbnail { get; set; }

        public List<string>? Categories { get; set; }
    }
}
