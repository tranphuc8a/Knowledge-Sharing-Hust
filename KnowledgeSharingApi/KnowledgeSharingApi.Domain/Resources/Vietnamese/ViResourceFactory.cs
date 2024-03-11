using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Resources.Vietnamese
{
    public class ViResourceFactory : IResourceFactory
    {
        private ViEntityResource? entityResource = null;
        private ViResponseResource? crudResource = null;
        private ViValidatorResource? validatorResource = null;

        public IResponseResource GetResponseResource()
        {
            return crudResource ?? new ViResponseResource();
        }

        public IEntityResource GetEntityResource()
        {
            return entityResource ?? new ViEntityResource();
        }

        public IValidatorResource GetValidatorResource()
        {
            return validatorResource ?? new ViValidatorResource();
        }
    }
}
