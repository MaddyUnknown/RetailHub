using Inventory.Core.Entities;
using Inventory.Core.Repository;
using Inventory.Infrastructure.Factories;
using Inventory.Infrastructure.Factories.Interface;
using Inventory.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure.Extensions
{
    public static class InfrastructureSetup
    {
        public static void AddInfraServices(this IServiceCollection serviceCollection)
        {
            #region Factories
            serviceCollection.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            #endregion

            #region Repositories
            serviceCollection.AddTransient<InventoryOrderItemRepository>();
            serviceCollection.AddTransient<ProductRepository>();

            serviceCollection.AddTransient<IAsyncRepository<InventoryOrderCancelReason>, InventoryOrderCancelReasonRepository>();
            serviceCollection.AddTransient<IAsyncRepository<InventoryOrderFullfillment>, InventoryOrderFullfillmentRepository>();
            serviceCollection.AddTransient<IAsyncRepository<InventoryOrderItem>>(provider => provider.GetRequiredService<InventoryOrderItemRepository>());
            serviceCollection.AddTransient<IAsyncRepository<InventoryOrderPayment>, InventoryOrderPaymentRepository>();
            serviceCollection.AddTransient<IAsyncRepository<InventoryOrder>, InventoryOrderRepository>();
            serviceCollection.AddTransient<IAsyncRepository<Product>>(provider => provider.GetRequiredService<ProductRepository>());
            serviceCollection.AddTransient<IAsyncRepository<Vendor>, VendorRepository>();

            serviceCollection.AddTransient<IInventoryOrderItemRepository>(provider => provider.GetRequiredService<InventoryOrderItemRepository>());
            serviceCollection.AddTransient<IProductRepository>(provider => provider.GetRequiredService<ProductRepository>());
            #endregion

        }
    }
}
