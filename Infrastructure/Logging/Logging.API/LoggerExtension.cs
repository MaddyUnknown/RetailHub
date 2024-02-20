using Logging.API.Correlation;
using Logging.API.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Logging.API
{
    public static class LoggerExtension
    {
        public static void AddApplicationLogging(this WebApplicationBuilder appBuilder)
        {
            //Correlation Services
            appBuilder.Services.AddSingleton<RequestCorrelationIdAccessor>();


            //Timer Services
            appBuilder.Services.AddSingleton<LogTimerAccessor>();





            appBuilder.Logging.ClearProviders();
            appBuilder.Host.UseSerilog((hostContext, serviceProvider, loggerConfiguration) =>
            {
                var requestCorrelatorAccessor = serviceProvider.GetRequiredService<RequestCorrelationIdAccessor>();
                var logTimerAccessor = serviceProvider.GetRequiredService<LogTimerAccessor>();

                loggerConfiguration
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.With(new RequestCorrelatorEnricher(requestCorrelatorAccessor))
                    .Enrich.With(new LogTimerEnricher(logTimerAccessor));
            });
        }
    }
}
