using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core.Enum
{
    public enum InventoryOrderStatusEnum : int
    {
        CREATED = 1,
        PARTIALLY_COMPLETED = 2,
        COMPLETED = 3,
        OVERDUE = 4,
        CANCELLED = 5
    }
}