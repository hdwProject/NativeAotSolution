using SqlSugar;

namespace Entity.Admin
{
    /// <summary>
    /// 管理员信息
    /// </summary>
    [SugarTable("AdminInfo")]
    public class AdminInfo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "Id", ColumnDescription = "主键Id")]
        public long Id { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(200)", ColumnName = "Name", ColumnDescription = "管理员名称")]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(200)", ColumnName = "Password", ColumnDescription = "密码")]
        public string? Password { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(500)", ColumnName = "Description", ColumnDescription = "描述")]
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(200)", ColumnName = "Email", ColumnDescription = "邮箱")] 
        public string? Email { get; set; } = string.Empty;

        /// <summary>
        /// 电话
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(50)", ColumnName = "Phone", ColumnDescription = "电话")] 
        public string? Phone { get; set; } = string.Empty;

        /// <summary>
        /// 省
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(50)", ColumnName = "Province", ColumnDescription = "省")]
        public string? Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(50)", ColumnName = "City", ColumnDescription = "市")] 
        public string? City { get; set; } = string.Empty;

        /// <summary>
        /// 区/县
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(50)", ColumnName = "Country", ColumnDescription = "区/县")] 
        public string? Country { get; set; } = string.Empty;

        /// <summary>
        /// 详细地址
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(200)", ColumnName = "Address", ColumnDescription = "详细地址")] 
        public string? Address { get; set; } = string.Empty;

        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(ColumnDataType = "bool", ColumnName = "IsDelete", ColumnDescription = "是否删除")]
        public bool? IsDelete { get; set; } = false;

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CreateDateTime", ColumnDescription = "创建时间")]
        public DateTime? CreateDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(ColumnName = "UpdateDateTime", ColumnDescription = "修改时间")]
        public DateTime? UpdateDateTime { get; set; } = DateTime.Now;

    }
}
