using Serilog.Core;
using Serilog.Events;

namespace Logging.API.Correlation
{
    public class RequestCorrelatorEnricher : ILogEventEnricher
    {
        private RequestCorrelationIdAccessor _accessor;

        public RequestCorrelatorEnricher(RequestCorrelationIdAccessor correlationIdAccessor)
        {
            _accessor = correlationIdAccessor ?? throw new ArgumentNullException(nameof(correlationIdAccessor));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = _accessor.RequestCorrelationId;

            if(correlationId != null)
            {
                var enrichProperty = propertyFactory
                .CreateProperty("RequestCorrelationId", correlationId.Id);

                logEvent.AddOrUpdateProperty(enrichProperty);
            }
        }
    }
}
