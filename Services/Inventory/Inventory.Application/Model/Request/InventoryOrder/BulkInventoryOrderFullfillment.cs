using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Model.Request.InventoryOrder
{
    public class BulkInventoryOrderFullfillment
    {
        public long? Id { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public List<ItemForBulkAddInventoryOrderFullfillment>? AddItems { get; set; }
        public List<ItemForBulkUpdateInventoryOrderFullfillment>? UpdateItems { get; set; }
        public List<ItemForBulkCancelInventoryOrderFullfillment>? CancelItems { get; set; }
    }

    public class ItemForBulkUpdateInventoryOrderFullfillment
    {
        public long? Id { get; set; }
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DeliveryDateTime { get; set; }
    }

    public class ItemForBulkCancelInventoryOrderFullfillment
    {
        public long? Id { get; set; }
    }
}
