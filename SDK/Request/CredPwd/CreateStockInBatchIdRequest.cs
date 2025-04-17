namespace SDK.Request.CredPwd
{
    /// <summary>
    /// 创建入库批次号请求
    /// </summary>
    public class CreateStockInBatchIdRequest : BaseRequest
    {
        /// <summary>
        /// 卡种Id
        /// </summary>
        public long CpkId { get; set; }

        /// <summary>
        /// 卡成本单价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 批次描述
        /// </summary>
        public string? Summary { get; set; }
    }
}
