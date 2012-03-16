using System;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using StarterTemplate.Core.Domain;
using StarterTemplate.Core.Extensions;

namespace StarterTemplate.Core.Data.EntityFramework
{
    public class EntityFrameworkRepository : EntityFrameworkReadOnlyRepository, IRepository 
    {
        private readonly DbContext _context;
        private readonly CurrentUserContext _currentUserContext;

        public EntityFrameworkRepository(
            DbContext context, 
            CurrentUserContext currentUserContext
            ) : base(context)
        {
            _context = context;
            _currentUserContext = currentUserContext;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : ImmutableEntityBase
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : ImmutableEntityBase
        {
            if (entity == null)
                return;

            _context.Set<TEntity>().Remove(entity);
        }

        public void Remove<TEntity>(long id) where TEntity : ImmutableEntityBase
        {
            var entity = Get<TEntity>(id);

            Remove(entity);
        }

        public void Remove<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase
        {
            foreach (var entity in All(where))
                Remove(entity);
        }

        public void SaveChanges()
        {
            var now = DateTime.Now;
            var memberId = _currentUserContext.SafeGet(c => c.Member.Id);

            foreach (var dbEntityEntry in _context.ChangeTracker.Entries())
            {
                var entityState = dbEntityEntry.State;

                if (entityState == EntityState.Added)
                {
                    var immutableEntity = dbEntityEntry.Entity as ImmutableEntityBase;
                    if (immutableEntity != null)
                    {
                        immutableEntity.CreatedByMemberId = memberId;
                        immutableEntity.CreatedDate = now;
                    }
                }

                if (entityState == EntityState.Added || entityState == EntityState.Modified)
                {
                    var mutableEntity = dbEntityEntry.Entity as MutableEntityBase;
                    if (mutableEntity != null)
                    {
                        mutableEntity.ModifiedByMemberId = memberId;
                        mutableEntity.ModifiedDate = now;
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}