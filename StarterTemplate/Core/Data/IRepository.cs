using System;
using System.Linq.Expressions;
using StarterTemplate.Core.Domain;

namespace StarterTemplate.Core.Data
{
    public interface IRepository : IReadOnlyRepository
    {
        void Add<TEntity>(TEntity entity) where TEntity : ImmutableEntityBase;
        void Remove<TEntity>(TEntity entity) where TEntity : ImmutableEntityBase;
        void Remove<TEntity>(long id) where TEntity : ImmutableEntityBase;
        void Remove<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase;
        
        void SaveChanges();
    }
}