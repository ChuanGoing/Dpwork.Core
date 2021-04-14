using System;

namespace Dpwork.Core.Features
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isIncrement">是否自增长的主键</param>
        public PrimaryKeyAttribute(bool isIncrement = true)
        {
            IsIncrement = isIncrement;
        }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIncrement { get; set; }
    }
}
