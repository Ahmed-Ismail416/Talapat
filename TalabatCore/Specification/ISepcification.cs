using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        
        // prop for oderby [OrderBy(P => P.Name)]
        public Expression<Func<T, object>> OrderyBy { get; set; }

        // prop for oderbydescending [OrderByDescending(P => P.Name)]
        public Expression<Func<T, object>> OrderyByDescending  { get; set; }

        //Pagination
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; } 
    }
}
