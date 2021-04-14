using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dpwork.Core.Message
{
    public class MessageMetadata : IDictionary<string, object>
    {
        private readonly Dictionary<string, object> _metas = new Dictionary<string, object>();

        public object this[string key]
        {
            get => _metas.ContainsKey(key) ? _metas[key] : null;
            set => _metas[key] = value;
        }

        public ICollection<string> Keys => _metas.Keys;

        public ICollection<object> Values => _metas.Values;

        public int Count => _metas.Count;

        public bool IsReadOnly => false;

        public void Add(string key, object value)
        {
            if (_metas.ContainsKey(key)) return;
            _metas.Add(key, value);
        }

        public void Add(KeyValuePair<string, object> item) => Add(item.Key, item.Value);

        public void Clear() => _metas.Clear();

        public bool Contains(KeyValuePair<string, object> item) => ContainsKey(item.Key);

        public bool ContainsKey(string key) => _metas.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _metas.ToArray().CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _metas.GetEnumerator();

        public bool Remove(string key) => _metas.Remove(key);

        public bool Remove(KeyValuePair<string, object> item) => _metas.Remove(item.Key);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _metas.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _metas.GetEnumerator();
    }
}
