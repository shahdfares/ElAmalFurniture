using ElAmal.Core.Entities;
using ElAmal.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace ElAmal.Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
        
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> spec)
        {
            var query = inputQuery;  // _dbContext.Set<T>();

            // Where 
            if(spec.Criteria is not null )   // P => P.Id == id 
                query = query.Where(spec.Criteria);


            // Sort 
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            else if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);


            // Include 
            query = spec.Include.Aggregate(query, (currentQuery, inputExpression) => currentQuery.Include(inputExpression));



            return query;
        }
    }
}
