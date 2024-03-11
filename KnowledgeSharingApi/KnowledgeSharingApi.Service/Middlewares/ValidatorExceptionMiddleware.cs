using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Middlewares
{
    /// <summary>
    /// Middleware bắt các ValidatorException ném ra từ AttributionValidation
    /// </summary>
    /// <param name="next"></param>
    /// Created: PhucTV (26/12/23)
    /// Modified: None
    public class ValidatorExceptionMiddleware(RequestDelegate next) : ExceptionMiddleware<ValidatorException>(next)
    {
        public async override Task ResolveException(HttpContext context, ValidatorException ex)
        {
            await Response(context, EStatusCode.BadRequest, new ApiResponse
            {
                StatusCode = EStatusCode.BadRequest,
                UserMessage = ex.Message,
                DevMessage = ex.Message
            });
        }
    }
}
