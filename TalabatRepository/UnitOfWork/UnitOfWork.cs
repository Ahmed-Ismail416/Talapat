using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatCore.Services.IUnitOfWork;
using TalabatRepository.Data;

namespace TalabatRepository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repostiory;
        public UnitOfWork(StoreContext Dbcontext)
        {
            _repostiory = new Hashtable();
            this._Dbcontext = Dbcontext;
        }

        public StoreContext _Dbcontext { get; }

        public async Task<int> CompleteAsync()
        => await _Dbcontext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        => await _Dbcontext.DisposeAsync();

        public IGenericRepository<TEntity> Repostiory<TEntity>() where TEntity : BaseEntity
        {
            var Type = typeof(TEntity).Name;
            if (!_repostiory.ContainsKey(Type))
            {
                var Repository = new GenericReposatory<TEntity>(_Dbcontext);
                _repostiory.Add(Type, Repository);
            }
            return _repostiory[Type] as IGenericRepository<TEntity>;


        }
    }
}
