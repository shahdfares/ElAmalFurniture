using ElAmal.Core.Entities;

namespace ElAmal.Core.Contract.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>;

        Task<int> CompleteAsync();
            
    }
}
