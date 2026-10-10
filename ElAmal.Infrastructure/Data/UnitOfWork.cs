using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Entities;
using ElAmal.Infrastructure.Data.Repository;
using System.Collections.Concurrent;

namespace ElAmal.Infrastructure.Data
{
    public class UnitOfWork  : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repository;
        private readonly ElAmalDbContext _dbContext;

        public UnitOfWork(ElAmalDbContext dbContext)
        {
            _dbContext = dbContext;
            _repository = new();
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>

            => (IGenericRepository<TEntity, TKey>) _repository.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbContext));
        public async Task<int> CompleteAsync()
          => await _dbContext.SaveChangesAsync();

        public void Dispose()
          => _dbContext.Dispose();
    }
}
