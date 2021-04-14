using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Repository.Dapper
{
    /// <summary>
    /// Dapper数据库上下文接口
    /// </summary>
    public interface IDapperDbContext: IDbContext
    {
        int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        IDataReader ExecuteReader(string sql, CommandBehavior commandBehavior, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<IDataReader> ExecuteReaderAsync(string sql, CommandBehavior commandBehavior, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        TResult ExecuteScalar<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<TResult> ExecuteScalarAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        IEnumerable<TResult> Query<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        TResult QueryFirst<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<TResult> QueryFirstAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        TResult QueryFirstOrDefault<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<TResult> QueryFirstOrDefaultAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        TResult QuerySingle<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<TResult> QuerySingleAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        TResult QuerySingleOrDefault<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<TResult> QuerySingleOrDefaultAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);

        SqlMapper.GridReader QueryMultiple(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default);
    }
}
