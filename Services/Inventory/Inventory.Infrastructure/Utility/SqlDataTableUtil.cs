using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Utility
{
    internal static class SqlDataTableUtil
    {
        internal static DataTable CreateLongDataTable(IEnumerable<long> list)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Value", typeof(long));
            foreach (int i in list)
            {
                table.Rows.Add(i);
            }

            return table;
        }
    }
}
