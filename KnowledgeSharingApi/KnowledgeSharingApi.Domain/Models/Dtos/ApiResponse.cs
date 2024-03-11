using KnowledgeSharingApi.Domains.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    /// <summary>
    /// Chuẩn hóa response trả về của API
    /// </summary>
    /// Created: PhucTV (27/12/23)
    /// Modified: None
    public class ApiResponse : IActionResult
    {
        public ApiResponse()
        {
            StatusCode = EStatusCode.Success;
            DevMessage = null;
            UserMessage = null;
            MoreInfo = null;
            TraceId = null;
            Body = null;
        }
        public ApiResponse(ServiceResult res)
        {
            FromServiceResult(res);
        }
        public EStatusCode StatusCode { get; set; }
        public string? DevMessage { get; set; }
        public string? UserMessage { get; set; }
        public string? MoreInfo { get; set; }
        public string? TraceId { get; set; }
        public object? Body { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)StatusCode;
            var errorResponse = new { StatusCode, UserMessage, DevMessage, MoreInfo, TraceId, Body };
            return response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(errorResponse));
        }

        /// <summary>
        /// Cập nhật thông tin ApiResponse dựa vào Service Result
        /// </summary>
        /// <param name="res"> Đối tượng ServiceResult mang dữ liệu </param>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        public void FromServiceResult(ServiceResult res)
        {
            StatusCode = res.StatusCode;
            DevMessage = res.DevMessage;
            UserMessage = res.UserMessage;
            Body = res.Data;
        }
    }
}
