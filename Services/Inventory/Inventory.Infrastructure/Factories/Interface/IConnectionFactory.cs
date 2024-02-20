using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Factories.Interface
{
    public interface IConnectionFactory<T, U>
    {
        T GetConnection(U option);
    }
}
