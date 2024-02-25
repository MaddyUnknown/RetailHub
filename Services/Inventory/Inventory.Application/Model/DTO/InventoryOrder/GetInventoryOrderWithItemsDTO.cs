using Inventory.Application.Model.DTO.InventoryOrderItem;
using Inventory.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Model.DTO.InventoryOrder
{
    public class GetInventoryOrderWithItemsDTO
    {
        public long Id { get; set; }
        public string? InventoryOrderNumber { get; set; }
        public string? ExternalOrderReferenceNumber { get; set; }
        public long? VendorId { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal? TotalCost { get; set; }
        public string? InventoryOrderStatus { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public IEnumerable<GetInventoryOrderItemForOrderDTO>? Items { get; set; }
    }
}
