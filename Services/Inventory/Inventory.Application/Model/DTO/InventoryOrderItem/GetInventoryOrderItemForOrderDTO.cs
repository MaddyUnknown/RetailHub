using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Model.DTO.InventoryOrderItem
{
    public class GetInventoryOrderItemForOrderDTO
    {
        public long? Id { get; set; }
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
