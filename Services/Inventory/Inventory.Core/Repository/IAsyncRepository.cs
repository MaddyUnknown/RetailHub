using Inventory.Core.Entities.Common;

namespace Inventory.Core.Repository
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<T?> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(long id);

    }
}
