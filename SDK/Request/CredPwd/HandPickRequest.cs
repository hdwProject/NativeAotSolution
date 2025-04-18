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
        public int? Num { get; set; }

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
        ///  buildCpd 传true时返回
        /// "CpdUrl": "https://mai.91kami.com/cpd/lu3hmk684kfch1n9wfauphnkx7eyrncqf78a39462da14270bd8666333404d81b.aspx" 
        ///  buildCpd传false时返回
        /// "CardPwdArr": [
        /// {
        ///     "c": "xx003",              // 卡号
        ///     "p": "123",                // 密码
        ///     "d": "2024-11-21 23:59:59" // 到期时间
        /// }
        /// </summary>
        public bool BuildCpd { get; set; } = false;

        /// <summary>
        /// 优先出库标识为“优先出售”的卡密，默认True
        /// </summary>
        public bool UsePriority { get; set; } = false;
    }
}
