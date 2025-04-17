namespace SDK.Request.CredPwd
{
    /// <summary>
    /// 获取卡种列表
    /// </summary>
    public class CardPwdRequest : BasePagerRequest
    {
        /// <summary>
        /// 卡种名称，支持模糊匹配
        /// </summary>
        public string? FilterName { get; set; }

        /// <summary>
        /// 卡种分类Id
        /// </summary>
        public int? ClassId { get; set; }
    }
}
