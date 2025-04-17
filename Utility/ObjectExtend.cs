using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Int32;

namespace Utility
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 将对象转换为字典
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>字典</returns>
        public static Dictionary<string, object> ToDictionary(this object? obj)
        {
            var dictionary = new Dictionary<string, object>();
            if (obj == null) return dictionary;
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    dictionary.Add(property.Name, value);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 序列化对象为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object? obj)
        {
            return obj == null ? string.Empty : obj.SerializeString();
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string TryString(this object? obj)
        {
            return obj is null ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// 将对象转换为整数
        /// <para>如果为对象为空，默然返回：0</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int TryInt(this object? obj)
        {
            var result = 0; 
            if (obj == null) return result; 
            TryParse(obj.ToString(), out result);
            return result;
        }
    }
}
