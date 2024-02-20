using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.API.Correlation
{
    internal class RequestCorrelationId : IRequestCorrelationId
    {
        private string _requestId = Guid.NewGuid().ToString();

        public string Id
        {
            get { return _requestId; }
            set { _requestId = value; }
        }
    }
}
