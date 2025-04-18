namespace SDK.Request.CredPwd
{
    /// <summary>
    /// 查询开放平台余额
    /// </summary>
    public class QueryDepositRequest : BaseRequest
    {
        /// <summary>
        /// 接入方订单编号
        /// </summary>
        public string? AppId { get; set; } = string.Empty;

    }
}
