namespace SDK.Request.CredPwd
{
    /// <summary>
    /// 手动提卡请求
    /// </summary>
    public class HandPickRequest : BaseRequest
    {
        /// <summary>
        /// 卡种Id
        /// </summary>
        public long CpkId { get; set; }

        /// <summary>
        /// 提卡数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 接入方订单编号
        /// </summary>
        public string? HandPickOrderId { get; set; } = string.Empty;

        /// <summary>
        /// 买家名称
        /// </summary>
        public string? Buyer { get; set; } = string.Empty;

        /// <summary>
        /// 生成完整卡券提取链接，默认False
        /// </summary>
        public bool BuildCpd { get; set; } = false;

        /// <summary>
        /// 优先出库标识为“优先出售”的卡密，默认True
        /// </summary>
        public bool UsePriority { get; set; } = false;
    }
}
