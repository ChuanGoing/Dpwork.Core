using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Dpwork.Core.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Dpwork.Core.Features
{
    public class MemoryCacheAttribute : AbstractInterceptorAttribute
    {
        private readonly bool _sliding;
        private readonly uint _timeMinutes;
        public MemoryCacheAttribute(bool sliding = false, uint minutes = 0)
        {
            _sliding = sliding;
            _timeMinutes = minutes;
        }


        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //var retType = context.ReturnValue.GetType();

            var retType = context.GetReturnParameter().Type;
            if (retType == typeof(void) || retType == typeof(Task) || retType == typeof(ValueTask))
            {
                await next(context);
                return;
            }
            var isAsync = context.IsAsync();

            //var returnType = retType;

            //if (isAsync)
            //{
            //    returnType = returnType.GenericTypeArguments.FirstOrDefault();
            //}

            string param = StringUtil.ObjectToString(context.Parameters);//await SerializeUtil.SerializeAsync(context.Parameters);

            string key = $"{context.ImplementationMethod.DeclaringType.FullName}.{context.ImplementationMethod.Name}.{param}";

            var cache = context.ServiceProvider.GetRequiredService<IMemoryCache>();

            if (cache.TryGetValue(key, out object value))
            {
                context.ReturnValue = value;
                return;
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
    }
}
