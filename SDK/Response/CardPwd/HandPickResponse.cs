using System.Text.Json.Serialization;

namespace SDK.Response.CardPwd
{
    /// <summary>
    /// 提卡返回结果
    /// </summary>
    public class HandPickResponse
    {
        /// <summary>
        /// 请求参数 “buildCpd” 为 true 时返回的文件名
        /// </summary>
        public bool? BuildCpd { get; set; } = false;

        /// <summary>
        /// 请求参数 “buildCpd” 为 true 返回链接地址
        /// </summary>
        public string? CpdUrl { get; set; }


        /// <summary>
        /// 请求参数 “buildCpd” 为 false 时返回卡密信息集合如：[ {"c":"卡号1", "p":"密码1", "d":"2024-11-15 23:59:59"},{"c":"卡号2", "p":"密码3", "d":"2024-11-15 23:59:59"} ]
        /// </summary>
        public List<CardPwdArr> CardPwdArr { get; set; } = [];

    }

    /// <summary>
    /// 卡密信息
    /// </summary>
    public record CardPwdArr
    {
        /// <summary>
        /// 卡号
        /// </summary>
        [JsonPropertyName("c")]
        public string? Card { get; set; }

        /// <summary>
        /// 密码
        /// </summary>

        [JsonPropertyName("p")]
        public string? Pass { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        [JsonPropertyName("d")]
        public DateTime? ExpireTime { get; set; }
    }

}
