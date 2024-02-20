using Logging.API.Correlation;
using Logging.API.Time;
using Microsoft.AspNetCore.Http;

namespace Logging.API
{
    public class ApplicationLoggerMiddleware
    {
        private RequestDelegate _next;
        private RequestCorrelationIdAccessor _requestCorrelationIdAccessor;
        private LogTimerAccessor _logTimerAccessor;

        public ApplicationLoggerMiddleware(RequestDelegate requestDelegate, RequestCorrelationIdAccessor requestCorrelationIdAccessor, LogTimerAccessor logTimerAccessor)
        {
            _next = requestDelegate;
            _requestCorrelationIdAccessor = requestCorrelationIdAccessor;
            _logTimerAccessor = logTimerAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            _requestCorrelationIdAccessor.RequestCorrelationId = new RequestCorrelationId();
            _logTimerAccessor.LogTimer = new LogTimer();

            _logTimerAccessor.LogTimer.ResetTimer();

            await _next(context);
        }
    }
}
