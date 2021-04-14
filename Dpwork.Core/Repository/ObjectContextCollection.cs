using System.Collections.Concurrent;

namespace Dpwork.Core.Repository
{
    public sealed class ObjectContextCollection : ConcurrentDictionary<string, ObjectContext>
    {
        private ObjectContextCollection() { }

        static ObjectContextCollection()
        {
            //Instance = new ObjectContextCollection();
        }

        public static ObjectContextCollection Instance
        {
            get { return Nested.Instance; }
        }

        private class Nested
        {
            internal static readonly ObjectContextCollection Instance = null;
            static Nested()
            {
                Instance = new ObjectContextCollection();
            }
        }
    }
}
