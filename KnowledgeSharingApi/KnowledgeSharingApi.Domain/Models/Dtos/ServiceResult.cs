using KnowledgeSharingApi.Domains.Enums;


namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    /// <summary>
    /// Giá trị trả về của các Service cho Controller
    /// </summary>
    /// Created: PhucTV (25/12/23)
    /// Modified: None
    public class ServiceResult
    {
        #region Constructors
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
        #endregion


        #region Attributes
        public bool IsSuccess { get; set; }
        public string UserMessage { get; set; }
        public string DevMessage { get; set; }
        public EStatusCode StatusCode { get; set; }
        public int RowEffect { get; set; }
        public object? Data { get; set; } 
        #endregion


        #region Static methods
        public static ServiceResult Success(string UserMessage = "", string DevMessage = "", object? Data = null)
            => CustomStatusCode(EStatusCode.Success, UserMessage, DevMessage, Data, true);
        public static ServiceResult BadRequest(string UserMessage = "", string DevMessage = "", object? Data = null)
            => CustomStatusCode(EStatusCode.BadRequest, UserMessage, DevMessage, Data);
        public static ServiceResult ServerError(string UserMessage = "", string DevMessage = "", object? Data = null)
            => CustomStatusCode(EStatusCode.ServerError, UserMessage, DevMessage, Data);
        public static ServiceResult UnAuthorized(string UserMessage = "", string DevMessage = "", object? Data = null)
            => CustomStatusCode(EStatusCode.Unauthorized, UserMessage, DevMessage, Data);
        public static ServiceResult Forbidden(string UserMessage = "", string DevMessage = "", object? Data = null)
            => CustomStatusCode(EStatusCode.Forbidden, UserMessage, DevMessage, Data);
        public static ServiceResult NotFound(string UserMessage = "", string DevMessage = "", object? Data = null)
            => CustomStatusCode(EStatusCode.NotFound, UserMessage, DevMessage, Data);

        public static ServiceResult CustomStatusCode(EStatusCode statusCode, string UserMessage = "", string DevMessage = "", object? Data = null, bool isSuccess = false)
        {
            if (string.IsNullOrEmpty(DevMessage))
            {
                DevMessage = UserMessage;
            }
            return new ServiceResult()
            {
                IsSuccess = isSuccess,
                StatusCode = statusCode,
                UserMessage = UserMessage,
                DevMessage = DevMessage,
                Data = Data
            };
        } 
        #endregion
    }
}
