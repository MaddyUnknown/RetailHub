using Inventory.Application.Extension;
using Inventory.Infrastructure.Extensions;
using Logging.API;
using UserContext.Core.Interface;
using UserContext.Infrastructure.WebAPI;

namespace Inventory.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            //Application specific dependency setup
            builder.AddApplicationLogging();

            builder.Services.AddScoped<IUserContext, WebAPIUserContext>(); // will need to re structure it
            builder.Services.AddInfraServices();
            builder.Services.AddApplicationService();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ApplicationLoggerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
    }
}