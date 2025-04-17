using System.Net;
using System.Reflection;
using Utility;

namespace SDK
{
    /// <summary>
    /// 接口请求类
    /// </summary>
    public static class ApiClient
    {
        /// <summary>
        /// 请求
        /// </summary>
        /// <typeparam name="T">返回的对象</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="method">方法名：比如 GET，POST，Delete，Put</param>
        /// <param name="formData">请求参数对象</param>
        /// <param name="headerDic">头部参数</param>
        /// <returns></returns>
        /// <exception cref="Exception">异常信息</exception>
        public static async Task<T?> Request<T>(string url, HttpMethod method, IReadOnlyList<KeyValuePair<string, string>> formData, Dictionary<string, object>? headerDic = null)
        {
            try
            {
                using var client = new HttpClient();
                var request = new HttpRequestMessage(method, url);
                if (headerDic is { Count: > 0 })
                {
                    foreach (var item in headerDic)
                    {
                        request.Headers.Add(item.Key, item.Value.ToString());
                    }
                }
                request.Headers.Add("Authorization", "Bearer Acpru33fe2cddrbhz9rfr2x5s5vmpf23b5p5mkebb2c9u9wf9");
                request.Headers.Add("ApiVersion", "1");
               
                //添加请求头为from表单提交
                request.Content = new FormUrlEncodedContent(formData);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response is { IsSuccessStatusCode: true, StatusCode: HttpStatusCode.OK })
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result.DeserializeObject<T?>();
                }

                throw new Exception($"请求失败，状态码：{response.StatusCode}");
            }
            catch (Exception e)
            {
                throw new Exception("请求失败：" + e);
            }
        }

        /// <summary>
        /// 根据对象获取字典
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static async Task<Dictionary<string, object>> GetDictionaryByObject(object? obj)
        {
            var dic = new Dictionary<string, object>();
            if (obj == null)
            {
                return dic;
            }

            //var jsonString = obj.SerializeString();
            //dic = jsonString.DeserializeObject<Dictionary<string, object>>() ?? new Dictionary<string, object>();

            var type = obj.GetType();
            // 获取所有实例属性（包括非公共）
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                try
                {
                    var value = prop.GetValue(obj);
                    if (value != null)
                    {
                        dic.Add(FirstCharToLower(prop.Name), value.TryString());
                    }
                }
                catch (Exception ex)
                {
                    SerilogHelper.Error(ex, ex.Message);
                    //Console.WriteLine($"读取 {prop.Name} 失败: {ex.Message}");
                }
            }

            var sign = SecurityHelper.AgisoSign(dic);
            //注意这里Sign的S是大写的
            dic.Add("sign", sign);
            return await Task.FromResult(dic);
        }

        /// <summary>
        /// 将字符串的首字母转换为小写（驼峰命名法）
        /// 示例："HelloWorld" -> "helloWorld"
        /// </summary>
        private static string FirstCharToLower(string input)
        {
            // 处理空值和空字符串
            if (string.IsNullOrEmpty(input))
                return input;

            // 方案1：使用字符操作（最高效）
            var chars = input.ToCharArray();
            chars[0] = char.ToLowerInvariant(chars[0]);
            return new string(chars);

            // 方案2：使用 Span（.NET Core 3.1+ 高性能方案）
            //return string.Create(input.Length, input, (span, str) =>
            //{
            //    str.AsSpan().CopyTo(span);
            //    span[0] = char.ToLowerInvariant(span[0]);
            //});

            // 方案3：使用字符串截取（简洁但性能稍差）
            //return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }
    }
}
