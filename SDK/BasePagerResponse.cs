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

        /// <summary>
        /// 总页数
        /// </summary>
        public int? TotalPageSize
        {
            get
            {
                if (TotalCount > 0)
                {
                    return TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
                }

                return 0;
            }
        }
    }
}
