using Dpwork.Core.Message.Args;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Message
{
    public abstract class MessageBus : IMessageBus
    {
        public MessageBus(IMessageHandleContext messageHandleContext)
        {
            MessageHandleContext = messageHandleContext;
        }

        public event EventHandler<MessagePublishedEventArgs> MessagePublished;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler Disposed;

        protected virtual void OnMessagePublished(MessagePublishedEventArgs e) => MessagePublished?.Invoke(this, e);
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e) => MessageReceived?.Invoke(this, e);

        public IMessageHandleContext MessageHandleContext { get; }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            //TODO:Aop实现
            DoPublished(message);
            OnMessagePublished(new MessagePublishedEventArgs(message));
        }

        public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : IMessage
        {
            //TODO:Aop实现
            await DoPublishedAsync(message, cancellationToken);
            OnMessagePublished(new MessagePublishedEventArgs(message));
        }

        public abstract void Subscribe<TMessage, TMessageHandle>()
            where TMessage : IMessage
            where TMessageHandle : IMessageHandle;


        protected abstract void DoPublished<TMessage>(TMessage message)
            where TMessage : IMessage;

        protected abstract Task DoPublishedAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage;

        //public void Dispose(bool disposing)
        //{
        //    //TODO:GC待处理
        //    Dispose();
        //    GC.SuppressFinalize(this);
        //}

        //public void Dispose()
        //{
        //    Disposed?.Invoke(this, EventArgs.Empty);
        //}
    }
}
