using Inventory.Application.Model.Request;
using Inventory.Application.Service;
using Inventory.Application.Service.Interface;
using Inventory.Application.Valitation;
using Inventory.Application.Valitation.Interface;
using Inventory.Application.Valitation.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application.Extension
{
    public static class ApplicationSetup
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            #region Validations
            services.AddTransient<IValidator<CancelInventoryOrder>, CancelInventoryOrderValidator>();
            services.AddTransient<IValidator<CreateInventoryOrderWithItems>, CreateInventoryOrderWithItemsValidator>();
            services.AddTransient<IValidator<GetInventoryOrderWithItemsById>, GetInventoryOrderWithItemsByIdValidator>();
            services.AddTransient<IValidator<UpdateInventoryOrderWithItems>, UpdateInventoryOrderWithItemsValidator>();



            #endregion

            #region Internal Services
            services.AddTransient<IValidatorService, ValidationService>();
            #endregion

            #region Services
            services.AddTransient<IInventoryOrderService, InventoryOrderService>();
            #endregion
        }

    }
}
