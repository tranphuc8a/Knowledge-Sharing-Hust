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
    public class ResponseExceptionMiddleware : ExceptionMiddleware<ResponseException>
    {
        public ResponseExceptionMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override async Task ResolveException(HttpContext context, ResponseException ex)
        {
            await Response(context, ex.StatusCode, new ApiResponse
            {
                StatusCode = ex.StatusCode,
                UserMessage = ex.UserMessage,
                DevMessage = ex.DevMessage,
                Body = ex.Body
            });
        }
    }
}
