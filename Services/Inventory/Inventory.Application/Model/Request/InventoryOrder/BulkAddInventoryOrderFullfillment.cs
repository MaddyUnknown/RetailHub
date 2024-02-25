using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Model.Request.InventoryOrder
{
    public class BulkAddInventoryOrderFullfillment
    {
        public long? Id { get; set; }
        public List<ItemForBulkAddInventoryOrderFullfillment>? AddItems { get; set; }
    }

    public class ItemForBulkAddInventoryOrderFullfillment
    {
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DeliveryDateTime { get; set; }
    }
}
