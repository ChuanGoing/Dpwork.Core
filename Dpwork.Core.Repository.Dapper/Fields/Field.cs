using System;

namespace Dpwork.Core.Repository.Dapper.Fields
{
    public class Field
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Alias { get; set; }

        public Field(string name, object value = null, string alias = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "无效的属性名");
            }
            Name = name;
            Value = value;
            Alias = alias;
        }
    }
}
