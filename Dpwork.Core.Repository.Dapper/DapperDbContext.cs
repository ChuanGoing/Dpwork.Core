using Dapper;
using Dpwork.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Repository.Dapper
{
    public abstract class DapperDbContext : IDapperDbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbBuilder _dbBuilder;
        private IDbConnection _connection;
        private IDbTransaction _tran;

        public DapperDbContext(IServiceProvider serviceProvider, string conStr)
        {
            _serviceProvider = serviceProvider;
            _dbBuilder = _serviceProvider.GetRequiredService<IDbBuilder>();
            _connection = _dbBuilder.CreateConnection(conStr);
        }

        public IRepository<TPrimaryKey, TEntity> GetRepository<TPrimaryKey, TEntity>()
           where TEntity : class, IEntity<TPrimaryKey>
        {
            var repo = _serviceProvider.GetService<IRepository<TPrimaryKey, TEntity>>();
            repo.SetDbContext(this);
            return repo;
        }

        public TRepository GetRepository<TRepository>() where TRepository : IRepository
        {
            var repo = _serviceProvider.GetRequiredService<TRepository>();
            repo.SetDbContext(this);
            return repo;
        }

        public void BeginTransaction(IsolationLevel level)
        {
            OpenConnection();
            if (_tran == null)
            {
                _tran = _connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _tran.Commit();
                CloseConnection();
                _tran = null;
            }
        }

        public void Rollback()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _tran.Rollback();
                CloseConnection();
                _tran = null;
            }
        }

        private void OpenConnection()
        {
            if (_connection == null)
            {
                throw new Exception("没有数据库连接对象");
            }
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (_connection == null)
            {
                throw new Exception("没有数据库连接对象");
            }
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        #region Dapper 
        //TODO:CommandDefinition 对象优化
        public virtual int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.Execute(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.ExecuteAsync(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual IDataReader ExecuteReader(string sql, CommandBehavior commandBehavior, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.ExecuteReader(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType), commandBehavior);
        }

        public virtual Task<IDataReader> ExecuteReaderAsync(string sql, CommandBehavior commandBehavior, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.ExecuteReaderAsync(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual TResult ExecuteScalar<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.ExecuteScalar<TResult>(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<TResult> ExecuteScalarAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.ExecuteScalarAsync<TResult>(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual IEnumerable<TResult> Query<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.Query<TResult>(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.QueryAsync<TResult>(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual TResult QueryFirst<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.QueryFirst<TResult>(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<TResult> QueryFirstAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.QueryFirstAsync<TResult>(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual TResult QueryFirstOrDefault<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.QueryFirstOrDefault<TResult>(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<TResult> QueryFirstOrDefaultAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.QueryFirstOrDefaultAsync<TResult>(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual TResult QuerySingle<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.QuerySingle<TResult>(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<TResult> QuerySingleAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.QuerySingleAsync<TResult>(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual TResult QuerySingleOrDefault<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.QuerySingleOrDefault<TResult>(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<TResult> QuerySingleOrDefaultAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.QuerySingleOrDefaultAsync<TResult>(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        public virtual SqlMapper.GridReader QueryMultiple(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _connection.QueryMultiple(sql: sql, param: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType);
        }

        public virtual Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default)
        {
            return _connection.QueryMultipleAsync(new CommandDefinition(commandText: sql, parameters: param, transaction: _tran, commandTimeout: commandTimeout, commandType: commandType, flags: flags, cancellationToken: cancellationToken));
        }

        #endregion

    }
}

