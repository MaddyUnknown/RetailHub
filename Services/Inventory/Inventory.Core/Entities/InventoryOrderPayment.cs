using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Core.Entities.Common;
using Inventory.Core.Enum;

namespace Inventory.Core.Entities
{
    public class InventoryOrderPayment : EntityBase
    {
        public long? InventoryOrderId { get; set; }
        public DateTime? PaymentTimeStamp { get; set; }
        public decimal? PaymentAmount { get; set; }
        public PaymentTypeEnum? PaymentType { get; set; }

    }
}
