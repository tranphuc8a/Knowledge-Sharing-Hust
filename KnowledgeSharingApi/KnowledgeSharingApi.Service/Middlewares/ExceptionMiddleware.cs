using KnowledgeSharingApi.Domains.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Middlewares
{
    /// <summary>
    /// Base Middleware theo kiến trúc xử lý Exception
    /// </summary>
    /// <typeparam name="T"> Loại Exception cần bắt để xử lý </typeparam>
    /// <param name="next"> Tham số bắt buộc của middleware </param>
    /// Created: PhucTV (26/12/23)
    /// Modified: None
    public abstract class ExceptionMiddleware<T>(RequestDelegate next) where T : Exception
    {
        protected readonly RequestDelegate _next = next;

        #region Override Invoke methods
        /// <summary>
        /// Hàm thực hiện Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: none
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Forward(context);
                await _next.Invoke(context);
                await Backward(context);
            }
            catch (T ex)
            {
                await ResolveException(context, ex);
            }
        }
        #endregion


        #region Steps of Template Method Invoke
        /// <summary>
        /// Bộ ba hàm xử lý logic của Middleware trong template method Invoke
        /// Forward: Logic xử lý trước khi pass middleware
        /// Backward: Logic xử lý khi return middleware
        /// ResolveException: Xử lý ngoại lệ trả về
        /// </summary>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        public virtual Task Forward(HttpContext context)
        {
            // do nothing, if want, override it
            return Task.CompletedTask;
        }
        public virtual Task Backward(HttpContext context)
        {
            // do nothing, if want, override it
            return Task.CompletedTask;
        }
        public abstract Task ResolveException(HttpContext context, T ex);
        #endregion


        /// <summary>
        /// Hàm thực hiện trả về response cho api
        /// </summary>
        /// <param name="context"> Ngữ cảnh gói tin HTTP </param>
        /// <param name="statusCode"> Mã trả về </param>
        /// <param name="data"> dữ liệu trả về </param>
        /// <returns></returns>
        /// Created: PhucTV (26/12/23)
        /// Modified: None
        public async Task Response(HttpContext context, EStatusCode statusCode, object data)
        {
            string jsonString = JsonSerializer.Serialize(data);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(jsonString);
        }
    }
}
