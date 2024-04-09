using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Request
{
    public interface IPostRequest : IRequest
    {
        /// <summary>
        /// Chuan bi du lieu de gui post voi formdata
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        IPostRequest PrepareFormData();

        /// <summary>
        /// Them du lieu vao formdata truoc khi gui di
        /// </summary>
        /// <param name="key"> key cua du lieu </param>
        /// <param name="value"> du lieu can them </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        IPostRequest AddFormData<T>(string key, T value);
    }
}
