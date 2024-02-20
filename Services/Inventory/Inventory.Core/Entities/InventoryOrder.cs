using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Core.Entities.Common;
using Inventory.Core.Enum;

namespace Inventory.Core.Entities
{
    public class InventoryOrder : EntityBase
    {
        public string? InventoryOrderNumber { get; set; }
        public string? ExternalOrderReferenceNumber { get; set; }
        public long? VendorId { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal? TotalCost { get; set; }
        public InventoryOrderStatusEnum? InventoryOrderStatus { get; set; }
    }
}