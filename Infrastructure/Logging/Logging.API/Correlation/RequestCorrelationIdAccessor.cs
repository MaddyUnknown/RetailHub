using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.API.Correlation
{
    public class RequestCorrelationIdAccessor
    {
        private static readonly AsyncLocal<RequestCorrelationIdHolder> _requestCorrelationId = new AsyncLocal<RequestCorrelationIdHolder>();

        public IRequestCorrelationId? RequestCorrelationId
        {
            get
            {
                return _requestCorrelationId.Value?.CorrelationId;
            }

            set
            {
                if(_requestCorrelationId.Value != null)
                {
                    _requestCorrelationId.Value.CorrelationId = null;
                }

                if(value != null)
                {
                    _requestCorrelationId.Value = new RequestCorrelationIdHolder { CorrelationId = value };
                }
            }
        }

        private class RequestCorrelationIdHolder
        {
            public IRequestCorrelationId? CorrelationId;
        }
    }
}
