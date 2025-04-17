using SDK.Enum;

namespace SDK.Response.CardPwd
{
    /// <summary>
    /// 获取卡种列表
    /// </summary>
    public class CardGetListResponse 
    {
        /// <summary>
        /// 卡种Id
        /// </summary>
        public long IdNo { get; set; }

        /// <summary>
        /// 卡种类型
        /// </summary>
        public CardTypeEnum? CardType { get; set; }

        /// <summary>
        /// 卡种名称
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int RemainingCount { get; set; }

        /// <summary>
        /// 已用数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int UsedCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 成本单价
        /// </summary>
        public decimal? PurPrice { get; set; }
    }
}
