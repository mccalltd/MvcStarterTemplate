using System;
using System.Linq;
using System.Linq.Expressions;
using StarterTemplate.Core.Domain;

namespace StarterTemplate.Core.Data
{
    public interface IRepository
    {
        IQueryable<TEntity> All<TEntity>() where TEntity : ImmutableEntityBase;
        IQueryable<TEntity> All<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase;
        TEntity Get<TEntity>(long id) where TEntity : ImmutableEntityBase;
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase;

        void Add<TEntity>(TEntity entity) where TEntity : ImmutableEntityBase;
        void Remove<TEntity>(TEntity entity) where TEntity : ImmutableEntityBase;
        void Remove<TEntity>(long id) where TEntity : ImmutableEntityBase;
        void Remove<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase;
        
        void SaveChanges();
    }
}