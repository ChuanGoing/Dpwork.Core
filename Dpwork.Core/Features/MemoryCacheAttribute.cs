﻿using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Dpwork.Core.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dpwork.Core.Features
{
    public class MemoryCacheAttribute : AbstractInterceptorAttribute
    {
        private readonly bool _sliding;
        private readonly uint _timeMinutes;
        private readonly bool _skipNull;
        private readonly string _key;
        private readonly int[] _paramsIndex;

        /// <summary>
        /// 实现aop memory cache
        /// </summary>
        /// <param name="key">指定缓存key,若没有指定缓存key时,根据paramsIndex指定得参数下标生成key,默认使用所有参数</param>
        /// <param name="sliding">是否滑动过期时间</param>
        /// <param name="minutes">过期时间(分钟)</param>
        /// <param name="skipNull">不缓存空值</param>
        /// <param name="paramsIndex">参数索引下标,使用指定参数集合作为CacheKey</param>
        public MemoryCacheAttribute(string key = "", bool sliding = false, uint minutes = 0, bool skipNull = true, params int[] paramsIndex)
        {
            _sliding = sliding;
            _timeMinutes = minutes;
            _skipNull = skipNull;
            _paramsIndex = paramsIndex;
            _key = key;
        }


        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var retType = context.GetReturnParameter().Type;
            if (retType == typeof(void) || retType == typeof(Task) || retType == typeof(ValueTask))
            {
                await next(context);
                return;
            }
            var isAsync = context.IsAsync();

            var returnType = retType;

            if (isAsync)
            {
                returnType = returnType.GenericTypeArguments.FirstOrDefault();
            }

            string key = GetCacheKey(context);

            var cache = context.ServiceProvider.GetRequiredService<IMemoryCache>();

            if (cache.TryGetValue(key, out object value))
            {
                if (!_skipNull || value != null)
                {
                    if (isAsync)
                    {
                        //TODO:优化
                        if (retType == typeof(Task<>).MakeGenericType(returnType))
                        {
                            //Task.FromResult(value)
                            context.ReturnValue = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(returnType).Invoke(null, new[] { value });
                        }
                        else if (retType == typeof(ValueTask<>).MakeGenericType(returnType))
                        {
                            //new ValueTask(value)
                            context.ReturnValue = Activator.CreateInstance(typeof(ValueTask<>).MakeGenericType(returnType), value);
                        }
                    }
                    else
                    {
                        context.ReturnValue = value;
                    }                    
                    return;
                }
            }

            await next(context);
            object returnValue;

            if (isAsync)
            {
                returnValue = await context.UnwrapAsyncReturnValue();
            }
            else
            {
                returnValue = context.ReturnValue;
            }
            if (_skipNull && returnValue == null)
            {
                return;
            }
            var sp = _timeMinutes == 0 ? TimeSpan.FromMinutes(10) : TimeSpan.FromMinutes(_timeMinutes);

            using var entry = cache.CreateEntry(key);
            if (_sliding)
            {
                entry.SetSlidingExpiration(sp);
            }
            else
            {
                entry.SetAbsoluteExpiration(sp);
            }
            entry.Value = returnValue;

        }

        private string GetCacheKey(AspectContext context)
        {
            if (!string.IsNullOrEmpty(_key)) return _key;
            //StringBuilder sb = new StringBuilder();
            var param = string.Empty;

            if (_paramsIndex == null || _paramsIndex.Count() == 0) param = StringUtil.ObjectToString("_", context.Parameters);
            else
            {
                for (int i = 0; i < context.Parameters.Length; i++)
                {
                    if (_paramsIndex.Any(a => a == i))
                    {

                        param += SerializeUtil.Serialize(context.Parameters[i]);
                        param += "_";
                    }
                }
                param = param.TrimEnd('_');
            }

            return $"{context.ImplementationMethod.DeclaringType.FullName}.{context.ImplementationMethod.Name}.{AESHelper.AesEncrypt(param)}";
        }
    }
}
