using System;
using System.Collections.Generic;

namespace Dpwork.Core
{
    public class DpworkContext
    {
        public readonly Dictionary<Type, Func<IServiceProvider, object>> Contexts;
        public DpworkContext()
        {
            Contexts = new Dictionary<Type, Func<IServiceProvider, object>>();
        }
    }
}
