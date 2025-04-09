using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;

namespace TalabatCore.Services.IUnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> Repostiory<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
        
    }
    
}
