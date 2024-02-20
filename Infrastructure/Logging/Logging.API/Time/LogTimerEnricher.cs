using Serilog.Core;
using Serilog.Events;

namespace Logging.API.Time
{
    public class LogTimerEnricher : ILogEventEnricher
    {
        private LogTimerAccessor _accessor;

        public LogTimerEnricher(LogTimerAccessor logTimerAccessor)
        {
            _accessor = logTimerAccessor ?? throw new ArgumentNullException(nameof(logTimerAccessor));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var timer = _accessor.LogTimer;

            if(timer != null)
            {
                var enrichProperty = propertyFactory
                .CreateProperty("TimeElapsed", timer.GetTimeElapsed);

                logEvent.AddOrUpdateProperty(enrichProperty);
            }
        }
    }
}
