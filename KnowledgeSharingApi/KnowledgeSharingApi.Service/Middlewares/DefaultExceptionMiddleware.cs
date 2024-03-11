using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Middlewares
{
    /// <summary>
    /// Middleware mặc định đặt ở trước mọi middleware khác
    /// Dùng để xử lý mọi Exception mà các middleware phía sau ném ra hoặc không bắt được
    /// </summary>
    /// <param name="next"></param>
    /// Created: PhucTV (26/12/23)
    /// Modified: None
    public class DefaultExceptionMiddleware(RequestDelegate next) : BaseMiddleware(next)
    {
        public override async Task ResolveException(HttpContext context, Exception ex)
        {
            await Response(context, EStatusCode.ServerError, new ApiResponse
            {
                StatusCode = EStatusCode.ServerError,
                UserMessage = ViConstantResource.SERVER_ERROR,
                DevMessage = ex.Message,
                Body = ex.ToString()
            });
        }
    }
}

