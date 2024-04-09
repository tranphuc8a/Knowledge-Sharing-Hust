using Microsoft.AspNetCore.Authentication;
using Pomelo.EntityFrameworkCore.MySql.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Request
{
    public interface IRequest
    {
        /// <summary>
        /// Thuc thi api lay ve ket qua
        /// </summary>
        /// <typeparam name="T"> Kieu du lieu tra ve cua api </typeparam>
        /// <returns> Ket qua tra ve cua API, null neu that bai </returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        Task<T?> Execute<T>();

        /// <summary>
        /// Dat content type cho request
        /// </summary>
        /// <param name="contentType"> content type cua request can gui </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        IRequest SetContentType(string contentType);

        /// <summary>
        /// Dat Url cho request
        /// </summary>
        /// <param name="url"> url cua request can gui </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        IRequest SetUrl(string url);

        /// <summary>
        /// Dat body cho request
        /// </summary>
        /// <param name="body"> body cua request can gui </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        IRequest SetBody(object body);

        /// <summary>
        /// Dat param cho request
        /// </summary>
        /// <param name="param"> param cua request can gui </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        IRequest SetParams(object param);

        /// <summary>
        /// Dat bearer token cho authentication truoc khi gui request
        /// </summary>
        /// <param name="token"> token can dat</param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        IRequest SetBearerAuthentication(string token);
    }
}
