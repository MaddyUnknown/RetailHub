using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Utils
{
    internal static class GuidUtil
    {
        internal static string GetUniqueId(string prefix)
        {
            var id = Guid.NewGuid().ToString("N").ToUpper();
            return prefix + id;
        }
    }
}
