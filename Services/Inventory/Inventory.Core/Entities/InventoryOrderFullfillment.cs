using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Core.Entities.Common;

namespace Inventory.Core.Entities
{
    public class InventoryOrderFullfillment : EntityBase
    {
        public long? InventoryOrderId { get; set; }
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ReceivedTimeStamp { get; set; }
    }
}
