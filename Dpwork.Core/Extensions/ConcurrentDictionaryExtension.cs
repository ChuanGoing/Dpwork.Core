using System.Collections.Concurrent;

namespace System.Collections.Generic
{
    public static class ConcurrentDictionaryExtension
    {
        public static void SafeRegister<TKey, TValue>(this ConcurrentDictionary<TKey, List<TValue>> registry, TKey key, TValue value)
        {
            if (registry.TryGetValue(key, out List<TValue> registryItem))
            {
                if (registryItem != null)
                {
                    if (!registryItem.Contains(value))
                    {
                        registry[key].Add(value);
                    }
                }
                else
                {
                    registry[key] = new List<TValue> { value };
                }
            }
            else
            {
                registry.TryAdd(key, new List<TValue> { value });
            }
        }

    }
}
