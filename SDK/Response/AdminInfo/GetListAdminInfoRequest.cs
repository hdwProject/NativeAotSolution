namespace SDK.Response.AdminInfo
{
    /// <summary>
    /// 获取管理员列表请求
    /// </summary>
    public class GetListAdminInfoRequest : BasePagerRequest
    {
        /// <summary>
        /// 管理员名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
    }
}
