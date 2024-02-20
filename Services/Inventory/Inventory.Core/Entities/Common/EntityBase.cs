using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core.Entities.Common
{
    public abstract class EntityBase
    {
        public long Id { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public string? UpdatedBy { get; protected set; }
        public DateTime? CreationDate { get; protected set; }
        public DateTime? UpdateDate { get; protected set; }
    }
}
