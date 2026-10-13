using ElAmal.Core.Entities;
using ElAmal.Core.Specifications;
using ElAmal.Core.Specifications.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Contract.Repository
{
    public interface IGenericRepository <TEntity, TKey> where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(bool WithTracking = false, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> spec, bool WithTracking = false, CancellationToken cancellationToken = default);
        Task<TEntity?> GetEntityAync(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity?> GetEntityWithSpecAync(ISpecification<TEntity, TKey> spec, CancellationToken cancellationToken = default);
        Task<int> GetCount(ISpecification<TEntity, TKey> spec);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
