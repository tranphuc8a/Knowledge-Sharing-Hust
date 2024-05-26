using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels
{
    public class UpdateCourseModel : UpdateKnowledgeModel
    {
        [PrivacyConverter]
        public EPrivacy? Privacy { get; set; }

        public string? Introduction { get; set; }

        public decimal? Fee { get; set; }

        public int? EstimateTimeInMinutes { get; set; }
    }
}
