using SDK.Response.CardPwd;

namespace SDK
{
    /// <summary>
    /// 分页返回结果
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class BasePagerResponse<TValue>
    {
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<TValue> List { get; set; } = []; 

        /// <summary>
        /// 分页页码
        /// </summary>
        public int? PageNo { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int? PageSize { get; set; } = 20;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int? TotalCount { get; set; } = 0;
    }
}
