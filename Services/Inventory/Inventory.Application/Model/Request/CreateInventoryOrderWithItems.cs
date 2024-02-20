using Inventory.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Model.Request
{
    public class CreateInventoryOrderWithItems
    {
        public string? ExternalOrderReferenceNumber { get; set; }
        public long? VendorId { get; set; }
        public decimal? Discount { get; set; }
        public decimal? ShippingFee { get; set; }
        public List<CreateInventoryOrderItemForOrder>? Items { get; set; }

    }

    public class CreateInventoryOrderItemForOrder
    {
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
