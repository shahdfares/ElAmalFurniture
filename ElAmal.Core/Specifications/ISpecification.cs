using ElAmal.Core.Entities;
using System.Linq.Expressions;

namespace ElAmal.Core.Specifications
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> Include { get; set; }
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool isPagenation { get; set; }
        
    }
}
