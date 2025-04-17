namespace SDK.Response.CardPwd
{
    /// <summary>
    /// 加卡返回结果
    /// </summary>
    public class StockInResponse
    {
        /// <summary>
        /// 添加失败数量
        /// </summary>
        public int? AddFailCount { get; set; }

        /// <summary>
        /// 添加成功数量
        /// </summary>
        public int? AddSuccessCount { get; set; }

        /// <summary>
        /// 覆盖成功数量
        /// </summary>
        public int? ReplaceSoldSuccessCount { get; set; }

        /// <summary>
        /// 覆盖失败数量
        /// </summary>
        public int? ReplaceUnSoldSuccessCount { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string? ResultMsg { get; set; }

    }

}
