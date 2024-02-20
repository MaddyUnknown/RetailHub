using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Exception
{
    public class ServiceException : System.Exception
    {

        public ServiceException(string message) : base(message) { }
    }
}
