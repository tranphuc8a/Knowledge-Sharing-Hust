using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.UserProfileModels
{
    public class UpdateGeneralInforModel
    {
        public string? FullName { get; set; }

        [UrlValidator(ErrorMessage = ViConstantResource.URL_FORMAT)]
        public string? Avatar { get; set; }

        [UrlValidator(ErrorMessage = ViConstantResource.URL_FORMAT)]
        public string? Cover { get; set; }
    }
}
