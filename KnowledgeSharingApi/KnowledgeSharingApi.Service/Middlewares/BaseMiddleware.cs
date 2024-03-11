using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Middlewares
{
    /// <summary>
    /// Abstract Middleware cho các middleware bắt chung mọi loại Exception
    /// Thường là các middleware validate trước khi cho gói tin đi qua
    /// </summary>
    /// Created: PhucTV (26/12/23)
    /// Modified: None
    public abstract class BaseMiddleware(RequestDelegate next) : ExceptionMiddleware<Exception>(next)
    {
    }
}
