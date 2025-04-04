using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Specification;

namespace TalabatRepository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        // _dbContext.Products.where(p => p.Id).Include(p => p.productBrand).Include(p => p.productType).FirstOrDefaultAsync();
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            //Where
            var Query = inputQuery;
            if (spec.Criteria != null)
                Query = Query.Where(spec.Criteria);

            //Orderby
            if (spec.OrderyBy != null)
            {
                Query = Query.OrderBy(spec.OrderyBy);
            }
            if (spec.OrderyByDescending != null)
            {
                Query = Query.OrderByDescending(spec.OrderyByDescending);
            }

            //Pagination
            if(spec.IsPagingEnabled)
            {
                Query = Query.Skip(spec.Skip).Take(spec.Take);
            }
            //Include
            Query = spec.Includes.Aggregate(Query,(CurrentQuery,IncludeExpression) =>CurrentQuery.Include(IncludeExpression )); //CurrentQuery.Include(p => p.productBrand).Include(p => p.productType)

            return Query;
        }
    }
}
