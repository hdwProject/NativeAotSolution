using SqlSugar;

namespace Entity.Admin
{
    /// <summary>
    /// 管理员信息
    /// </summary>
    public class AdminInfo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 省
        /// </summary>
        public string? Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// 区/县
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; } = DateTime.Now;

    }
}
