namespace Dpwork.Core.Repository.Dapper.Filters
{
    /// <summary>
    /// 相等过滤
    /// </summary>
    public class Equal : Filter
    {
        public Equal(string field, object value)
            : base(field)
        {
            Value = value;
        }
        public object Value { get; }
    }
}
