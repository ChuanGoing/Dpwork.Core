using System;

namespace Dpwork.Core.Features
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string _name)
        {
            Name = _name;
        }

        public string Name { get; private set; }
    }
}
