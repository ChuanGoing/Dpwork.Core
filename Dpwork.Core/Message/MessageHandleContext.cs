using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Message
{
    public abstract class MessageHandleContext : IMessageHandleContext
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ConcurrentDictionary<Type, List<Type>> registrations = new ConcurrentDictionary<Type, List<Type>>();

        public MessageHandleContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public bool HandleRegistered(Type message, Type messageHandle)
        {
            if (registrations.TryGetValue(message, out List<Type> handleTypeList))
            {
                return handleTypeList != null && handleTypeList.Contains(messageHandle);
            }
            return false;
        }

        public bool HandleRegistered<TMessage, TMessageHandle>()
            where TMessage : IMessage
            where TMessageHandle : IMessageHandle
            => HandleRegistered(typeof(TMessage), typeof(TMessageHandle));

        public void RegisterHandle(Type message, Type messageHandle)
        {
            registrations.SafeRegister(message, messageHandle);
        }

        public void RegisterHandle<TMessage, TMessageHandle>()
            where TMessage : IMessage
            where TMessageHandle : IMessageHandle
            => RegisterHandle(typeof(TMessage), typeof(TMessageHandle));

        public async Task HandleAsync(IMessage message, CancellationToken cancellationToken = default)
        {
            //TODO:待验证
            var messageType = message.GetType();
            if (registrations.TryGetValue(messageType, out List<Type> handleTypeList)
                && handleTypeList?.Count > 0)
            {
                using var scope = _serviceProvider.CreateScope();
                foreach (var handleType in handleTypeList)
                {
                    var handle = scope.ServiceProvider.GetRequiredService(handleType) as IMessageHandle;
                    if (handle.CanHandle(message))
                    {
                        await handle.HandleAsync(message, cancellationToken);
                    }
                }
            }
        }
    }
}
