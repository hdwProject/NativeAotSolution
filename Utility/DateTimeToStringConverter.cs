using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utility
{
    /// <summary>
    /// 时间Json序列化帮助类
    /// </summary>
    public class DateTimeToStringConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 反序列化日期时间
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // 这里可以添加反序列化的逻辑，但通常我们只关注序列化
            if (reader.TokenType != JsonTokenType.String)
                return reader.GetDateTime();
            return DateTime.TryParse(reader.GetString(), out var dt) ? dt : reader.GetDateTime();
        }

        /// <summary>
        /// 序列化日期时间
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // 自定义日期时间格式
            var formattedDate = value.ToString("yyyy-MM-dd HH:mm:ss"); // 例如："2023-04-01 12:30:00"
            writer.WriteStringValue(formattedDate);
        }
    }
}
