using System;
using System.Linq;
using System.Linq.Expressions;
using StarterTemplate.Core.Domain;

namespace StarterTemplate.Core.Data
{
    public interface IReadOnlyRepository
    {
        IQueryable<TEntity> All<TEntity>() where TEntity : ImmutableEntityBase;
        IQueryable<TEntity> All<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase;
        TEntity Get<TEntity>(long id) where TEntity : ImmutableEntityBase;
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : ImmutableEntityBase;
    }
}