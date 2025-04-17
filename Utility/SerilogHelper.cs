using Serilog;

namespace Utility
{
    /// <summary>
    /// 打印日志帮助类
    /// </summary>
    public static class SerilogHelper
    {
        /// <summary>
        /// 输出一条简单的日志信息
        /// </summary>
        /// <param name="message"></param>
        public static void Information(string message)
        {
            Log.Logger.Information(message);
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="propertyValue"></param>
        public static void Information<T>(string message, T propertyValue)
        {
            Log.Logger.Information(message, propertyValue);
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="propertyValue0"></param>
        /// <param name="propertyValue1"></param>
        public static void Information<T0, T1>(string message, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Logger.Information(message, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="propertyValue0"></param>
        /// <param name="propertyValue1"></param>
        /// <param name="propertyValue2"></param>
        public static void Information<T0, T1, T2>(string message, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Logger.Information(message, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Information(string messageTemplate, params object?[]? propertyValues)
        {
            Log.Logger.Information(messageTemplate, propertyValues);
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Information(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            Log.Logger.Information(exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageTemplate"></param>
        public static void Error(Exception? exception, string messageTemplate)
        {
            Log.Logger.Error(exception, messageTemplate);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValue"></param>
        public static void Error<T>(Exception? exception, string messageTemplate, T propertyValue)
        {
            Log.Logger.Error(exception, messageTemplate, propertyValue);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValue0"></param>
        /// <param name="propertyValue1"></param>
        public static void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Log.Logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValue0"></param>
        /// <param name="propertyValue1"></param>
        /// <param name="propertyValue2"></param>
        public static void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Log.Logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Error(Exception? exception, string messageTemplate, params object?[]? propertyValues)
        {
            Log.Logger.Error(exception, messageTemplate, propertyValues);
        }
    }
}
