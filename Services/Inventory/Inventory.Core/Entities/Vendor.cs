using Inventory.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core.Entities
{
    public class Vendor : EntityBase
    {
        public string? VendorCode { get; set; }
        public string? VendorName { get; set; }
        public string? VendorAddress { get; set; }
    }
}
