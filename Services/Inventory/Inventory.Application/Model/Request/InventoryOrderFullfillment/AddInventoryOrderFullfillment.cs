using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Model.Request.InventoryOrderFullfillment
{
    public class AddInventoryOrderFullfillment
    {
        public long? InventoryOrderId { get; set; }
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DeliveryDateTime { get; set; }
    }
}
