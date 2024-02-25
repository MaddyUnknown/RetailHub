using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Model.Request
{
    public class AddInventoryOrderFullfillment
    {
        public long? Id { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public List<ItemForAddInventoryOrderFullfillment>? Items { get; set; }
    }

    public class ItemForAddInventoryOrderFullfillment
    {
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
    }

    public class AddInvetoryOrderFullfillmentItem
    {
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }

    public class UpdateInventoryOrderFullfillmentItem
    {
        public long? Id { get; set; }
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }

    public class CancelInventoryOrderFullfullmentItem
    {
        public long? Id { get; set; }
    }
}
