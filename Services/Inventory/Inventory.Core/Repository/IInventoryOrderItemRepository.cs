using Inventory.Core.Entities;

namespace Inventory.Core.Repository
{
    public interface IInventoryOrderItemRepository : IAsyncRepository<InventoryOrderItem>
    {
        Task<IEnumerable<InventoryOrderItem>> GetByIdListAsync(IEnumerable<long> ids);

        Task<IEnumerable<InventoryOrderItem>> GetByOrderIdAsync(long orderId);

    }
}
