using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Entities;
using ElAmal.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ElAmal.Infrastructure.Data.Repository
{
    public class GenericRepository<TEntity, TKey> (ElAmalDbContext _dbContext) : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool WithTracking = false, CancellationToken cancellationToken = default)
             => WithTracking ? await _dbContext.Set<TEntity>().ToListAsync(cancellationToken)
              : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

        public async Task<TEntity?> GetEntityAync(TKey id, CancellationToken cancellationToken = default)
             => await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
             => await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        public void Update(TEntity entity)
             => _dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity)
             => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> spec, bool WithTracking = false, CancellationToken cancellationToken = default)
             => await ApplySpecification(spec).AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetEntityWithSpecAync(ISpecification<TEntity, TKey> spec, CancellationToken cancellationToken = default)
           => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> GetCount(ISpecification<TEntity, TKey> spec)
           => await ApplySpecification(spec).CountAsync();

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TKey> spec)
        
           =>  SpecificationEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), spec);
        
    }
}
