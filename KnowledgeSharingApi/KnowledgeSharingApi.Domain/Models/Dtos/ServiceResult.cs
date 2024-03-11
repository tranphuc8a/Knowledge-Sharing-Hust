using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    /// <summary>
    /// Giá trị trả về của các Service cho Controller
    /// </summary>
    /// Created: PhucTV (25/12/23)
    /// Modified: None
    public class ServiceResult
    {
        public ServiceResult()
        {
            IsSuccess = true;
            UserMessage = "";
            DevMessage = "";
            StatusCode = EStatusCode.Success;
            RowEffect = 0;
            Data = null;
        }
        public ServiceResult(int rowEffect) : this()
        {
            RowEffect = rowEffect;
        }
        public ServiceResult(int rowEffect, object? Data) : this(rowEffect)
        {
            this.Data = Data;
        }
        public bool IsSuccess { get; set; }
        public string UserMessage { get; set; }
        public string DevMessage { get; set; }
        public EStatusCode StatusCode { get; set; }
        public int RowEffect { get; set; }
        public object? Data { get; set; }

        public static ServiceResult Success(string UserMessage = "", string DevMessage = "", object? Data = null)
        {
            if (String.IsNullOrEmpty(DevMessage))
            {
                DevMessage = UserMessage;
            }
            return new ServiceResult()
            {
                IsSuccess = true,
                StatusCode = EStatusCode.Success,
                UserMessage = UserMessage,
                DevMessage = DevMessage,
                Data = Data
            };
        }
        public static ServiceResult BadRequest(string UserMessage = "", string DevMessage = "", object? Data = null)
        {
            if (String.IsNullOrEmpty(DevMessage))
            {
                DevMessage = UserMessage;
            }
            return new ServiceResult()
            {
                IsSuccess = false,
                StatusCode = EStatusCode.BadRequest,
                UserMessage = UserMessage,
                DevMessage = DevMessage,
                Data = Data
            };
        }
        public static ServiceResult ServerError(string UserMessage = "", string DevMessage = "", object? Data = null)
        {
            if (String.IsNullOrEmpty(DevMessage))
            {
                DevMessage = UserMessage;
            }
            return new ServiceResult()
            {
                IsSuccess = false,
                StatusCode = EStatusCode.ServerError,
                UserMessage = UserMessage,
                DevMessage = DevMessage,
                Data = Data
            };
        }
    }
}
