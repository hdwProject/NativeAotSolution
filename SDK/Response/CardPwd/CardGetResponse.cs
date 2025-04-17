namespace SDK.Response.CardPwd
{
    /// <summary>
    /// 获取卡种列表
    /// </summary>
    public class CardGetResponse
    {
        public long IdNo { get; set; }

        public long CardType { get; set; }

        public string? Title { get; set; }

        public long RemainingCount { get; set; }

        public long TotalCount { get; set; }

        public long UsedCount { get; set; }

        public DateTime? CreateTime { get; set; }

        public decimal? PurPrice { get; set; }
    }
}
