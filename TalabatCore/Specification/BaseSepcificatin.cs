using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specification
{
    public class BaseSepcificatin<T> : ISpecification<T> where T : BaseEntity
    {


        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderyBy { get; set; }
        public Expression<Func<T, object>> OrderyByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; }



        // Just for Using Includes
        public BaseSepcificatin()
        {

        }
        //For Using Criteriac And Includes
        public BaseSepcificatin(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;

        }

        //for setting the value of OrderyBy
        public void OrderBy(Expression<Func<T, object>> expression)
        {
            OrderyBy = expression;
        }
        // for setting the value of OrderyByDescending
        public void OrderByDescending(Expression<Func<T, object>> expression)
        {
            OrderyByDescending = expression;
        }

        //for Pagination
        public void ApplyPagination(int skip , int take)
        {
            IsPagingEnabled = true;
            Take = take;
            Skip = skip;
        }
    }
}
