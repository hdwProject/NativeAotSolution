using SqlSugar;
using System.Linq.Expressions;

namespace Repositories
{
    /// <summary>
    /// 泛型仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> where TEntity : class, new()
    {
        // 用于其他表操作
        public ITenant Tenant { get; }

        public SqlSugarScopeProvider Db { get; private set; }

        public BaseRepository(ISqlSugarClient scope)
        {
           
            Tenant = scope.AsTenant();
            Db = Tenant.GetConnectionScopeWithAttr<TEntity>();
        }

        /// <summary>
        /// 切换上下文，不传参切换到实体租户
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public ISqlSugarClient ChangeEntityDb(object configId = null)
        {
            if (configId != null)
            {
                Db = Tenant.GetConnectionScope(configId);
            }
            else
            {
                Db = Tenant.GetConnectionScopeWithAttr<TEntity>();
            }
            return Db;
        }

       



        public async Task<TEntity> Insert(TEntity entity)
        {
            return await Db.Insertable(entity).ExecuteReturnEntityAsync();
        }

        public async Task<int> Insert(List<TEntity> list)
        {
            return await Db.Insertable(list).ExecuteCommandAsync();
        }

        public async Task<int> Update(TEntity entity)
        {
            return await Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }

        public async Task<int> Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content)
        {
            return await Db.Updateable(content).Where(predicate).ExecuteCommandAsync();
        }

        public async Task<int> Delete(TEntity entity)
        {
            return await Db.Deleteable(entity).ExecuteCommandAsync();
        }

        public async Task<int> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Deleteable(predicate).ExecuteCommandAsync();
        }

        public async Task<TEntity> FindEntity(object keyValue)
        {
            return await IQueryable().InSingleAsync(keyValue);
        }

        public async Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return await IQueryable().FirstAsync(predicate);
        }

        public ISugarQueryable<TEntity> IQueryable()
        {
            return Db.Queryable<TEntity>();
        }

        public ISugarQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return IQueryable().Where(predicate);
        }

        public ISugarQueryable<TEntity> IQueryable(string strSql)
        {
            return Db.SqlQueryable<TEntity>(strSql);
        }

    }
}
