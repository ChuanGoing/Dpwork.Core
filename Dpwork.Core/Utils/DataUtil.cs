using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading;

namespace Dpwork.Core.Utils
{
    public class LazyConcurrentDictionary<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dic;

        public LazyConcurrentDictionary()
        {
            _dic = new ConcurrentDictionary<TKey, Lazy<TValue>>();
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            var lazyResult = _dic.GetOrAdd(key, k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication));

            return lazyResult.Value;
        }
    }
    
    public class RandomUtil
    {
        /// <summary>
        /// 线程安全随机数生成器
        /// </summary>
        /// <returns></returns>
        public static Random ThreadSafeRandom()
        {
            return ThreadLocalRandom.NewRandom.Value;
        }
    }

    internal class ThreadLocalRandom
    {
        /// <summary>
        /// 强随机数生成器
        /// </summary>
        private static readonly RNGCryptoServiceProvider _global = new RNGCryptoServiceProvider();

        public static ThreadLocal<Random> NewRandom = new ThreadLocal<Random>(() =>
        {
            var buffer = new byte[4];
            _global.GetBytes(buffer);
            return new Random(BitConverter.ToInt32(buffer, 0));
        });
    }
}
