using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using StarterTemplate.Core.Domain;
using StarterTemplate.Core.Extensions;

namespace StarterTemplate.Core.Data.EntityFramework
{
    public class EntityFrameworkRepository : IRepository 
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrentUserContext _currentUserContext;

        public EntityFrameworkRepository(
            ApplicationDbContext context, 
            CurrentUserContext currentUserContext
            )
        {
            _context = context;
            _currentUserContext = currentUserContext;
        }

        public IQueryable<TEntity> All<TEntity>() where TEntity : ImmutableEntityBase
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> All<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase
        {
            return All<TEntity>().Where(where);
        }

        public TEntity Get<TEntity>(long id) where TEntity : ImmutableEntityBase
        {
            return All<TEntity>().SingleOrDefault(x => x.Id == id);
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase
        {
            return All<TEntity>().FirstOrDefault(where);
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
            var member = First<Member>(m => m.EmailAddress == _currentUserContext.Username);

            foreach (var dbEntityEntry in _context.ChangeTracker.Entries())
            {
                var entityState = dbEntityEntry.State;

                if (entityState == EntityState.Added)
                {
                    var immutableEntity = dbEntityEntry.Entity as ImmutableEntityBase;
                    if (immutableEntity != null)
                    {
                        immutableEntity.CreatedByMemberId = member.SafeGet(m => m.Id);
                        immutableEntity.CreatedDate = now;
                    }
                }

                if (entityState == EntityState.Added || entityState == EntityState.Modified)
                {
                    var mutableEntity = dbEntityEntry.Entity as MutableEntityBase;
                    if (mutableEntity != null)
                    {
                        mutableEntity.ModifiedByMemberId = member.SafeGet(m => m.Id);
                        mutableEntity.ModifiedDate = now;
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}