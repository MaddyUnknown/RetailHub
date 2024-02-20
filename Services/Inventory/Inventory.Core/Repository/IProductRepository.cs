using Inventory.Core.Entities;

namespace Inventory.Core.Repository
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<IEnumerable<Product>> GetByIdListAsync(IEnumerable<long> ids);
    }
}
