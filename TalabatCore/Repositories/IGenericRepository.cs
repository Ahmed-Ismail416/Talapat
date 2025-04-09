using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Specification;

namespace TalabatCore.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Old Way Without Specification
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetbyIdAsync(int id);
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        #endregion


        #region New Way With Specification
        public Task<IReadOnlyList<T>> GetAllWithSpecAsync( ISpecification<T> spec);
        public Task<T> GetbyIdWithSpecAsync(ISpecification<T> spec);

        //Pagination Count
        public Task<int> GetCountWithSpecAsync(ISpecification<T> spec);
        
        #endregion

    }
}
