namespace SDK.Request.CredPwd
{
    /// <summary>
    /// 手动加卡请求
    /// 700:循环卡:[{"CardNo":"卡号","Pwd":"卡密"}]
    /// 800:套卡：[{"CardNo":"卡号"}]
    /// 820:唯一卡：[{"CardNo":"卡号","Pwd":"卡密"}]
    /// 840:重复卡：[{"CardNo":"卡号","Pwd":"卡密"}]
    /// 850:图片卡：[{"Url":"图片URL地址","Md5":"图片M5d值","OriName":"图片名称"}]
    /// </summary>
    public class StockInRequest : BaseRequest
    {
        /// <summary>
        /// 入库批次号
        /// </summary>
        public string? StockInBatchId { get; set; }

        /// <summary>
        /// 卡券内容，内容格式需要根据卡种类型，具体查看备注
        /// <para>
        /// 700:循环卡:[{"CardNo":"卡号","Pwd":"卡密"}]
        /// 800:套卡：[{"CardNo":"卡号"}]
        /// 820:唯一卡：[{"CardNo":"卡号","Pwd":"卡密"}]
        /// 840:重复卡：[{"CardNo":"卡号","Pwd":"卡密"}]
        /// 850:图片卡：[{"Url":"图片URL地址","Md5":"图片M5d值","OriName":"图片名称"}]
        /// </para>
        /// </summary>
        public string? Data { get; set; }

        /// <summary>
        /// 优先出售，默认False
        /// </summary>
        public bool IsUseFirst { get; set; } = false;

        /// <summary>
        /// 覆盖已存在的，默认False
        /// </summary>
        public bool AllowReplace { get; set; } = false;

        /// <summary>
        /// 注：卡种开启到期日时必填，其他忽略
        /// </summary>
        public DateTime? ExpireTime { get; set; }
      
    }

    /// <summary>
    /// 添加卡券内容
    /// <para>700:循环卡:[{"CardNo":"卡号","Pwd":"卡密"}]</para>
    /// <para>820:唯一卡：[{"CardNo":"卡号","Pwd":"卡密"}]</para>
    /// <para>840:重复卡：[{"CardNo":"卡号","Pwd":"卡密"}]</para>
    /// </summary>
    public class Data700And820And840
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string? CardNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Pwd { get; set; }
    }

    /// <summary>
    /// 添加卡券内容
    /// <para>800:套卡：[{"CardNo":"卡号"}]</para>
    /// </summary>
    public class Data800
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string? CardNo { get; set; }
    }

    /// <summary>
    /// 添加卡券内容
    /// <para>850:图片卡：[{"Url":"图片URL地址","Md5":"图片M5d值","OriName":"图片名称"}]</para>
    /// </summary>
    public class Data850
    {
        /// <summary>
        /// 图片URL地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 图片M5d值
        /// </summary>
        public string? Md5 { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string? OriName { get; set; }
    }

    /// <summary>
    /// 添加卡券内容
    /// <para>850:图片卡：[{"Url":"图片URL地址","Md5":"图片M5d值","OriName":"图片名称"}]</para>
    /// </summary>
    public class Data850Image
    {
        /// <summary>
        /// 图片URL地址
        /// </summary>
        public byte[]? Url { get; set; }
        
    }
}
