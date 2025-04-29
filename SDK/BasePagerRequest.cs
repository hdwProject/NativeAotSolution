using SqlSugar;

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
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序方式（默认是倒序）
        /// </summary>
        public OrderByType OrderByType { get; set; } = OrderByType.Desc;

        /// <summary>
        /// 排序字段（默认是Id）
        /// </summary>
        public string? OrderByField { get; set; } = "Id";
    }
}
