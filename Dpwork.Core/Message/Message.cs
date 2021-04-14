using System;

namespace Dpwork.Core.Message
{
    public abstract class Message : IMessage
    {
        private readonly MessageMetadata metadata = new MessageMetadata();

        public Message()
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public DateTime Timestamp { get; set; }

        public MessageMetadata Metadata => metadata;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var message = obj as Message;
            if (message == null) return false;

            return Id.Equals(message.Id);
        }

        public override string ToString() => Id;

        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
