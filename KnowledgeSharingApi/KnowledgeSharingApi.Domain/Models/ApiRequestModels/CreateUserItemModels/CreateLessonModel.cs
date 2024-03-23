using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels
{
    public class CreateLessonModel : CreatePostModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ESTIMATION_EMPTY)] 
        public int? EstimateTimeInMinutes { get; set; }
    }
}
