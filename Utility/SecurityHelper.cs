using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public class SecurityHelper
    {
        /// <summary>
        /// Md5摘要
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <returns></returns>
        public static string Md5Encrypt(string text)
        {
            var fromData = Encoding.UTF8.GetBytes(text);
            var targetData = MD5.HashData(fromData);
            return Convert.ToHexStringLower(targetData);
        }

        /// <summary>
        /// Md5摘要
        /// </summary>
        /// <param name="bytes">需要加密的字符串</param>
        /// <returns></returns>
        public static string Md5Encrypt(byte[] bytes)
        {
            var targetData = MD5.HashData(bytes);
            return Convert.ToHexStringLower(targetData);
        }

        /// <summary>
        /// 获取阿奇索参数签名字符串
        /// </summary>
        /// <param name="dic">参数字典</param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public static string AgisoSign(IDictionary<string, object> dic, string clientSecret = "28ueddkc9sv3emwzzyt8nyxgscuapph8")
        {
            IDictionary<string, object> sortedParams = new SortedDictionary<string, object>(dic, StringComparer.Ordinal);
            var str = new StringBuilder();
            foreach (var item in sortedParams)
            {
                str.Append(item.Key).Append(item.Value);
            }
            //头加入AppSecret
            str.Insert(0, clientSecret);
            //尾加入AppSecret
            str.Append(clientSecret); 
            var encodeStr = Md5Encrypt(str.ToString());
            return encodeStr;
        }
    }
}
