using System.Collections.Generic;

namespace ScreeningAutomation.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Storage;
    using Models.Base;

    public interface IRepository<T> where T : IBaseEntity
    {
        Task<T> SaveAsync(T entity);
        Task SaveRangeAsync(IEnumerable<T> entities);
        IQueryable<T> GetAll();        
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<int> idList);
        IDbContextTransaction BeginTransaction();
    }
}
