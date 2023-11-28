using System.Linq.Expressions;

namespace DAL.Abstractions;

public interface IRepository<TEntity, TEntityId>
    where TEntity : class, IIdEntity<TEntityId>
    where TEntityId : notnull
{
    IQueryable<TEntity> Entities { get; }

    TEntity GetById(TEntityId id);

    Task<TEntity> GetByIdAsync(TEntityId id);
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "");

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    Task RemoveByIdAsync(TEntityId id);

    void Remove(TEntity entity);

    Task<int> SaveChangesAsync();
    void Dispose();
}
