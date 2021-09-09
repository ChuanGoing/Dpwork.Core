using System;

namespace Dpwork.Core.Utils
{
    public class DateUtil
    {
        /// <summary>
        /// 服务器本地时间(秒)
        /// </summary>
        /// <returns></returns>
        public static long LocalTimeSeconds()
        {
            return DateTimeOffset.Now.ToLocalTime().ToUnixTimeSeconds();
        }

        /// <summary>
        /// 服务器本地时间(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long LocalTimeMilliseconds()
        {
            return DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 服务器本地当前日期天(秒)
        /// </summary>
        /// <returns></returns>
        public static long LocalDateSenconds()
        {
            return new DateTimeOffset(DateTimeOffset.Now.LocalDateTime.Date).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Unix最大时间(s)
        /// </summary>
        /// <returns></returns>
        public static long MaxUnixTimeSenconds()
        {
            return DateTimeOffset.MaxValue.ToUnixTimeSeconds();
        }
    }

}
