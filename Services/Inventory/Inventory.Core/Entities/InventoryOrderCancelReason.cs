using Inventory.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core.Entities
{
    public class InventoryOrderCancelReason : EntityBase
    {
        public long? InventoryOrderId { get; set; }
        public string? OrderCancelReason { get; set; }
    }
}
