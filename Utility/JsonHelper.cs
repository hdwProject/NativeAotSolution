using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Utility
{
    /// <summary>
    /// Json序列化帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Json序列化配置
        /// </summary>
        private static readonly JsonSerializerOptions? CustomJsonSerializerOptions = new()
        {
            // 使用自定义格式（如 "yyyy-MM-dd HH:mm:ss"）
            Converters = { new DateTimeToStringConverter() },

            // 整齐打印
            WriteIndented = true,

            // 忽略值为Null的属性
            // 设置Json字符串支持的编码，默认情况下，序列化程序会转义所有非 ASCII 字符。 即，会将它们替换为 \uxxxx，其中 xxxx 为字符的 Unicode
            // 代码。 可以通过设置Encoder来让生成的json字符串不转义指定的字符集而进行序列化 下面指定了基础拉丁字母和中日韩统一表意文字的基础Unicode 块
            // (U+4E00-U+9FCC)。 基本涵盖了除使用西里尔字母以外所有西方国家的文字和亚洲中日韩越的文字
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), //JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),

            // 反序列化不区分大小写
            PropertyNameCaseInsensitive = true,

            // 驼峰命名
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
           
            IncludeFields = false,

            // 对字典的键进行驼峰命名
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,

            // 序列化的时候忽略null值属性
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

            // 忽略只读属性，因为只读属性只能序列化而不能反序列化，所以在以json为储存数据的介质的时候，序列化只读属性意义不大
            IgnoreReadOnlyFields = true,

            // 不允许结尾有逗号的不标准json
            AllowTrailingCommas = false,

            // 不允许有注释的不标准json
            ReadCommentHandling = JsonCommentHandling.Disallow,

            // 允许在反序列化的时候原本应为数字的字符串（带引号的数字）转为数字 这里的字符串是指数字类型的字符串，比如"123"、"123.45"、"-123.45"等
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
           
            // 处理循环引用类型，比如Book类里面有一个属性也是Book类
            //加上这会出现$Id的属性
            //ReferenceHandler = ReferenceHandler.Preserve
        };

        /// <summary>
        /// 序列化对象为json字符串
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="obj">字符串</param>
        /// <returns></returns>
        public static string SerializeString<T>(this T? obj)
        {
            return obj == null ? string.Empty : JsonSerializer.Serialize(obj, CustomJsonSerializerOptions);
        }
        
        /// <summary>
        /// 序列化对象为json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string SerializeString(this object obj)
        {
            return JsonSerializer.Serialize(obj, CustomJsonSerializerOptions);
        }

        /// <summary>
        /// 反序列化json字符串为对象
        /// </summary>
        /// <typeparam name="TValue">对象</typeparam>
        /// <param name="json">字符串</param>
        /// <returns></returns>
        public static TValue? DeserializeObject<TValue>(this string json)
        {
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<TValue>(json, CustomJsonSerializerOptions);
        }
    }
}
