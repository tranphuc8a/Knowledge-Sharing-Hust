using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels
{
    public class UpdateProfileModel
    {
        public string? FullName { get; set; }

        [UrlValidator(ErrorMessage = ViConstantResource.URL_FORMAT)]
        public string? Avatar { get; set; }

        [UrlValidator(ErrorMessage = ViConstantResource.URL_FORMAT)]
        public string? Cover { get; set; }
        public string? Nickname { get; set; }
        public string? Bio { get; set; }

        [GenderConverter]
        public EGender? Gender { get; set; }

        [CustomDateTimeConverter]
        [GreaterThanTodayValidator(ErrorMessage = ViConstantResource.DOB_GREATER_THAN_TODAY)]
        public DateTime? DateOfBirth { get; set; }

        [PhoneValidator(ErrorMessage = ViConstantResource.PHONE_FORMAT)]
        public string? PhoneNumber { get; set; }

        [EmailValidator(ErrorMessage = ViConstantResource.EMAIL_FORMAT)]
        public string? ContactEmail { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }

        [UrlValidator(ErrorMessage = ViConstantResource.URL_FORMAT)]
        public string? SocialLink { get; set; }
        public string? School { get; set; }
        public string? Profession { get; set; }
        public double? Cpa { get; set; }
        public string? Grade { get; set; }
        public string? Class { get; set; }
        public string? Job { get; set; }
    }
}
