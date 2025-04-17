namespace SDK
{
    /// <summary>
    /// 分页请求类
    /// </summary>
    public class BasePagerRequest : BaseRequest
    {
        /// <summary>
        /// 分页页码
        /// </summary>
        public int? PageIndex { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int? PageSize { get; set; } = 20;
    }
}
