using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core.Enum
{
    public enum InventoryOrderStatusEnum : int
    {
        Created = 1,
        PartiallyCompleted = 2,
        Completed = 3,
        Overdue = 4,
        Cancelled = 5
    }
}