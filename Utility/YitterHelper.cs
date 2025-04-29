using Yitter.IdGenerator;

namespace Utility
{
    /// <summary>
    /// 雪花算法帮助类
    /// </summary>
    public static class YitterHelper
    {
        /// <summary>
        /// 雪花算法，获取18位不重复的数字
        /// </summary>
        /// <returns></returns>
        public static long GetYitId()
        {
            var newId = YitIdHelper.NextId();
            return newId;
        }

        /// <summary>
        /// 初始化，程序启动的时候必须调佣此方法，不然直接合并使用，速度非常的慢
        /// </summary>
        public static void InitIdHelper()
        {
            //var options = new IdGeneratorOptions(1);
            //// 创建 IdGeneratorOptions 对象，可在构造函数中输入 WorkerId：
            var options = new IdGeneratorOptions(1)
            {
                WorkerIdBitLength = 6, // 默认值6，限定 WorkerId 最大值为2^6-1，即默认最多支持64个节点。
                SeqBitLength = 8 // 默认值6，限制每毫秒生成的ID个数。若生成速度超过5万个/秒，建议加大 SeqBitLength 到 10。
            };
            // options.BaseTime = Your_Base_Time; // 如果要兼容老系统的雪花算法，此处应设置为老系统的BaseTime。
            // ...... 其它参数参考 IdGeneratorOptions 定义。
            // 保存参数（务必调用，否则参数设置不生效）：
            YitIdHelper.SetIdGenerator(options);
        }
    }
}
