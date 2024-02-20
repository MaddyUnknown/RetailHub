using Inventory.Application.Model.DTO.InventoryOrder;
using Inventory.Application.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Service.Interface
{
    public interface IInventoryOrderService
    {
        Task<GetInventoryOrderWithItemsDTO> CreateInventoryOrder(CreateInventoryOrderWithItems model);
        Task<GetInventoryOrderWithItemsDTO> UpdateInventoryOrder(UpdateInventoryOrderWithItems model);
    }
}
