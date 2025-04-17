namespace SDK
{
    /// <summary>
    /// 基础请求类
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public virtual string Timestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
    }
}
