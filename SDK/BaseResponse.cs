using System.Text.Json.Serialization;

namespace SDK
{
    /// <summary>
    /// 基础返回类
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class BaseResponse<TData> 
    {
        /// <summary>
        /// 返回码
        /// </summary>
        [JsonPropertyName("Error_Code")]
        public int? ErrorCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        [JsonPropertyName("Error_Msg")]
        public string? Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public TData Data { get; set; } = default!;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess => string.IsNullOrWhiteSpace(Message);

        /// <summary>
        /// 
        /// </summary>
        public string? AllowRetry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? RequestId { get; set; }
    }
    
}
