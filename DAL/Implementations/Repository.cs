using DAL.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Implementations
{
    public class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
        where TEntity : class, IIdEntity<TEntityId>
        where TEntityId : notnull
    {
        protected ApplicationDBContext Context;
        protected DbSet<TEntity> DbSet { get; }
        public IQueryable<TEntity> Entities { get; }
        private bool disposed = false;

        public Repository(ApplicationDBContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
            Entities = DbSet;
        }

        public TEntity GetById(TEntityId id)
        {
            return Entities.SingleOrDefault(e => e.Id.Equals(id));
        }

        public async Task<TEntity> GetByIdAsync(TEntityId id)
        {
            return await Entities.SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public IQueryable<TEntity> Find(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "")
        {
            IQueryable<TEntity> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            return query;
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }


        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public async Task RemoveByIdAsync(TEntityId id)
        {
            TEntity entity = await DbSet.FindAsync(id);
            Remove(entity);
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose of any managed resources here
                    Context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
