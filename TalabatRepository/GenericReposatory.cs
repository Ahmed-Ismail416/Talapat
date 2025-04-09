using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatCore.Specification;
using TalabatRepository.Data;

namespace TalabatRepository
{
    public class GenericReposatory<T> : IGenericRepository<T> where T : BaseEntity
    {
        public StoreContext _dbcontext { get; }
        public GenericReposatory(StoreContext storeContext)
        {
            _dbcontext = storeContext;
        }

        #region GenericReposatory without specification
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _dbcontext.Products.Include(p=>p.ProductBrand).Include(p=>p.ProductType).ToListAsync();
            }
            else
            return await _dbcontext.Set<T>().ToListAsync();
        }
        public async Task<T> GetbyIdAsync(int id)
        => await _dbcontext.Set<T>().FindAsync(id);
        public async Task AddAsync(T entity)
        => await _dbcontext.Set<T>().AddAsync(entity);

        public void Update(T entity)
        => _dbcontext.Set<T>().Update(entity);

        public void Delete(T entity)
        => _dbcontext.Set<T>().Remove(entity);


        #endregion



        #region GenericReposatory with specification

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetbyIdWithSpecAsync(ISpecification<T> spec)
        => await ApplySpecification(spec).FirstOrDefaultAsync();
        #endregion


        #region ApplySpecification
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }




        #endregion
    }
}
