﻿using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels
{
    public class TokenModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ACCESS_TOKEN_EMPTY)]
        public string? AccessToken { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.REFRESH_TOKEN_EMPTY)]
        public string? RefreshToken { get; set; }
    }
}
