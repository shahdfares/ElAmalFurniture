using ElAmal.Core.Entities;
using System.Linq.Expressions;

namespace ElAmal.Core.Specifications
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; } = null!;
        public List<Expression<Func<TEntity, object>>> Include { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool isPagenation { get; set; }
        public Expression<Func<TEntity, object>> OrderBy { get ; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }

        public BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
           // Include Initialization
        }

        public BaseSpecification()
        {
             // Set Criteria null
            // Include Initialization
        }

        public void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderBy = orderByDescExpression;
        }

        public void ApplyPagenation(int skip, int take)
        {
             Skip = skip;
            Take = take;
            isPagenation = true;
        }

    }
}
