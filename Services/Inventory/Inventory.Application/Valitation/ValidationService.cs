using Inventory.Application.Valitation.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application.Valitation
{
    public class ValidationService : IValidatorService
    {
        private IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<ValidationResult> Validate<T>(T obj)
        {
            IValidator<T> validator = _serviceProvider.GetRequiredService<IValidator<T>>();

            return await validator.IsValid(obj);
        }
    }
}