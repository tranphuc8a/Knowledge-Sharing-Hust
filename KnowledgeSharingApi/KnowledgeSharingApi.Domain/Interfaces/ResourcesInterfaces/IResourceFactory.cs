using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces
{
    public interface IResourceFactory
    {
        /// <summary>
        /// Tạo và lấy instance của EntityResource
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (10/3/24)
        /// Modified: None
        IEntityResource GetEntityResource();

        /// <summary>
        /// Tạo và lấy instance của CrudResource
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (10/3/24)
        /// Modified: None
        IResponseResource GetResponseResource();

        /// <summary>
        /// Tạo và lấy instance của ValidatorResource
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (10/3/24)
        /// Modified: None
        IValidatorResource GetValidatorResource();
    }
}
