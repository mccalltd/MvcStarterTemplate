using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using StarterTemplate.Core.Domain;

namespace StarterTemplate.Core.Data.EntityFramework
{
    public class EntityFrameworkReadOnlyRepository : IReadOnlyRepository 
    {
        private readonly DbContext _context;

        public EntityFrameworkReadOnlyRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> All<TEntity>() where TEntity : ImmutableEntityBase
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> All<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase
        {
            return All<TEntity>().Where(@where);
        }

        public TEntity Get<TEntity>(long id) where TEntity : ImmutableEntityBase
        {
            return All<TEntity>().SingleOrDefault(x => x.Id == id);
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase
        {
            return All<TEntity>().FirstOrDefault(@where);
        }
    }
}