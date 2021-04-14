using System;

namespace Dpwork.Core.Features
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {

        public TableAttribute(string _tableName)
        {
            Name = _tableName;
        }
        public string Name { get; private set; }
    }
}
