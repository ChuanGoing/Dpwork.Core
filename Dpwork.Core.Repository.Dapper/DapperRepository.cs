using Dpwork.Core.Domain;
using Dpwork.Core.Features;
using Dpwork.Core.Repository.Dapper.Commands;
using Dpwork.Core.Repository.Dapper.Fields;
using Dpwork.Core.Repository.Dapper.Filters;
using Dpwork.Core.Repository.Dapper.Parameters;
using Dpwork.Core.Utils;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Repository.Dapper
{
    public class DapperRepository<TPrimaryKey, TEntity> :
          ICommandRepository<TPrimaryKey, TEntity>,
          IQueryRepository<TPrimaryKey, TEntity>
          where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly ICommandBuilder _commandBuilder;
        protected IDapperDbContext DbContext { get; private set; }


        public DapperRepository(ICommandBuilder CommandBuilder)
        {
            _commandBuilder = CommandBuilder;
        }

        public void SetDbContext<TDbContext>(TDbContext dbContext) where TDbContext : IDbContext
        {
            if (dbContext is IDapperDbContext dapperDbContext)
            {
                DbContext = dapperDbContext;
            }
        }

        public virtual void BeginTransaction(IsolationLevel level = IsolationLevel.Unspecified)
        {
            DbContext.BeginTransaction(level);
        }

        public virtual void Commit()
        {
            DbContext.Commit();
        }

        public virtual void Rollback()
        {
            DbContext.Rollback();
        }


        protected virtual ObjectContext GetObjectContext<T>()
        {
            var type = typeof(T);

            string tableKey = ObjectContext.GetTableKey(typeof(T));

            return ObjectContextCollection.Instance.GetOrAdd(tableKey, entity => new ObjectContext(type));
        }

        #region ICommandRepository

        public virtual TPrimaryKey Create(TEntity entity)
        {
            var com = CreateCommand(entity);
            return DbContext.QuerySingleOrDefault<TPrimaryKey>(com.SqlString, com.Parameters);
        }
        public virtual Task<TPrimaryKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var com = CreateCommand(entity);
            return DbContext.QuerySingleOrDefaultAsync<TPrimaryKey>(sql: com.SqlString, param: com.Parameters, cancellationToken: cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            var com = UpdateCommand(entity);
            DbContext.Execute(com.SqlString, com.Parameters);
        }
        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var com = UpdateCommand(entity);
            return DbContext.ExecuteAsync(sql: com.SqlString, param: com.Parameters, cancellationToken: cancellationToken);
        }

        public virtual void DeleteByKey(TPrimaryKey key)
        {
            var com = DeleteCommand(new List<TPrimaryKey> { key });
            DbContext.Execute(com.SqlString, com.Parameters);
        }

        public virtual Task DeleteByKeyAsync(TPrimaryKey key, CancellationToken cancellationToken = default)
        {
            var com = DeleteCommand(new List<TPrimaryKey> { key });
            return DbContext.ExecuteAsync(sql: com.SqlString, param: com.Parameters, cancellationToken: cancellationToken);
        }

        public virtual void DeleteByKeys(IEnumerable<TPrimaryKey> keys)
        {
            var com = DeleteCommand(keys);
            DbContext.Execute(com.SqlString, com.Parameters);
        }

        public virtual Task DeleteByKeysAsync(IEnumerable<TPrimaryKey> keys, CancellationToken cancellationToken = default)
        {
            var com = DeleteCommand(keys);
            return DbContext.ExecuteAsync(sql: com.SqlString, param: com.Parameters, cancellationToken: cancellationToken);
        }

        public virtual void Delete(TEntity entity)
        {
            DeleteByKey(entity.Id);
        }

        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return DeleteByKeyAsync(entity.Id, cancellationToken);
        }

        public virtual void Delete(IEnumerable<TEntity> entitys)
        {
            DeleteByKeys(entitys.Select(e => e.Id));
        }

        public virtual Task DeleteAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default)
        {
            return DeleteByKeysAsync(entitys.Select(e => e.Id), cancellationToken);
        }

        #endregion

        #region IQueryRepository       

        public TEntity Find(TPrimaryKey key)
        {
            var com = GetCommand(key);
            return DbContext.QuerySingleOrDefault<TEntity>(com.SqlString, com.Parameters);
        }

        public Task<TEntity> FindAsync(TPrimaryKey key, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => Find(key), cancellationToken);
        }
        public IEnumerable<TEntity> FindAll()
        {
            var obj = GetObjectContext<TEntity>();
            FieldsCollection fields = new FieldsCollection();
            bool skip;
            foreach (var prop in obj.Properties)
            {
                skip = false;
                foreach (var attr in prop.Attributes)
                {
                    if (attr is IgnoreAttribute)
                    {
                        skip = true;
                    }
                }
                if (!skip) fields.Add(new Field(prop.Info.GetColumnName(), null, !prop.Info.GetColumnName().Equals(prop.Info.Name) ? prop.Info.Name : null));
            }

            QueryParameter queryParameter = new QueryParameter(fields);
            SqlCommand command = _commandBuilder.QueryCommand(obj.Table, queryParameter);
            return DbContext.Query<TEntity>(command.SqlString, command.Parameters).AsQueryable();
        }
        public Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => FindAll(), cancellationToken);
        }

        //TODO:未完待续
        //public virtual bool Exist(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return false;
        //}
        //public Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        //{
        //    return Task.Run(() => Exist(predicate), cancellationToken);
        //}

        //public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, bool>> orderPredicate = null)
        //{
        //    return null;
        //}
        //public Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, bool>> orderPredicate = null, CancellationToken cancellationToken = default)
        //{
        //    return Task.Run(() => FindAll(predicate, orderPredicate), cancellationToken);
        //}

        #endregion

        #region 

        #endregion

        #region Command

        public SqlCommand GetCommand(TPrimaryKey key)
        {
            var obj = GetObjectContext<TEntity>();
            FieldsCollection fields = new FieldsCollection();
            List<Filter> filters = new List<Filter>();
            bool skip;
            foreach (var prop in obj.Properties)
            {
                skip = false;
                foreach (var attr in prop.Attributes)
                {
                    if (attr is IgnoreAttribute)
                    {
                        skip = true;
                    }
                    else if (attr is PrimaryKeyAttribute keyAttr)
                    {
                        filters.Add(new Equal(prop.Info.GetColumnName(), key));
                    }
                }
                if (!skip) fields.Add(new Field(prop.Info.GetColumnName(), null, !prop.Info.GetColumnName().Equals(prop.Info.Name) ? prop.Info.Name : null));
            }

            QueryParameter queryParameter = new QueryParameter(fields, filters);
            return _commandBuilder.QueryCommand(obj.Table, queryParameter, count: 1);
        }

        public SqlCommand CreateCommand(TEntity entity)
        {
            var obj = GetObjectContext<TEntity>();
            FieldsCollection fields = new FieldsCollection();
            bool skip;
            bool isKeyIncrement = false;
            foreach (var prop in obj.Properties)
            {
                skip = false;
                foreach (var attr in prop.Attributes)
                {
                    if (attr is IgnoreAttribute)
                    {
                        skip = true;
                    }
                    else if (attr is PrimaryKeyAttribute attribute)
                    {
                        if (attribute.IsIncrement)
                        {
                            isKeyIncrement = true;
                        }
                    }
                }
                if (!skip || !isKeyIncrement) fields.Add(new Field(prop.Info.GetColumnName(), prop.Info.GetValue(entity)));
            }
            var com = _commandBuilder.InsertCommand(obj.Table, fields, isKeyIncrement);
            return com;
        }

        public SqlCommand UpdateCommand(TEntity entity)
        {
            var obj = GetObjectContext<TEntity>();
            FieldsCollection fields = new FieldsCollection();
            List<Filter> filters = new List<Filter>();
            bool skip;
            foreach (var prop in obj.Properties)
            {
                skip = false;
                var value = prop.Info.GetValue(entity);
                foreach (var attr in prop.Attributes)
                {
                    if (attr is PrimaryKeyAttribute keyAttr)
                    {
                        filters.Add(new Equal(prop.Info.GetColumnName(), value));
                        skip = true;
                    }
                }
                if (!skip) fields.Add(new Field(prop.Info.GetColumnName(), value));
            }
            return _commandBuilder.UpdateCommand(obj.Table, fields, filters);
        }

        public SqlCommand DeleteCommand(IEnumerable<TPrimaryKey> keys)
        {
            var obj = GetObjectContext<TEntity>();
            List<Filter> filters = new List<Filter>();
            foreach (var prop in obj.Properties)
            {
                foreach (var attr in prop.Attributes)
                {
                    if (attr is PrimaryKeyAttribute)
                    {
                        filters.Add(new Equal(prop.Info.GetColumnName(), keys));
                        break;
                    }
                }
            }
            return _commandBuilder.DeleteCommand(obj.Table, filters);
        }

        #endregion     
    }

    public static class PropertyExt
    {
        public static readonly LazyConcurrentDictionary<string, string> propertyCache = new LazyConcurrentDictionary<string, string>();

        /// <summary>
        /// 获取字段名称
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo pi)
        {
            return propertyCache.GetOrAdd($"{pi.DeclaringType.FullName}.{pi.Name}", (k) =>
            {
                var attr = pi.GetCustomAttribute<ColumnAttribute>();

                return attr == null ? pi.Name : attr.Name;
            });
        }
    }


}
