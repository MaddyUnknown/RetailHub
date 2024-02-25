using Inventory.Application.Model.Request;
using Inventory.Application.Valitation.Interface;
using Inventory.Core.Entities;
using Inventory.Core.Enum;
using Inventory.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Valitation.Validator
{
    internal class CancelInventoryOrderValidator : IValidator<CancelInventoryOrder>
    {
        private IAsyncRepository<InventoryOrder> _inventoryOrderRepository;

        public CancelInventoryOrderValidator(IAsyncRepository<InventoryOrder> inventoryOrderRepository)
        {
            _inventoryOrderRepository = inventoryOrderRepository ?? throw new ArgumentNullException(nameof(inventoryOrderRepository));
        }

        public async Task<ValidationResult> IsValid(CancelInventoryOrder obj)
        {
            ValidationResult result = new();
            result.IsValid = true;

            // Order Id Validation
            if (obj.Id == null || obj.Id < 1)
            {
                result.Errors.Add($"Id must be greater than or equal to 1.");
                result.IsValid = false;
            }
            else if (obj.Id != null)
            {
                var order = await _inventoryOrderRepository.GetByIdAsync(obj.Id.Value);
                if (order == null)
                {
                    result.Errors.Add($"Inventory Order with Id {obj.Id} does not exist.");
                    result.IsValid = false;
                }
                else if (order.InventoryOrderStatus != InventoryOrderStatusEnum.CREATED)
                {
                    result.Errors.Add($"Cannot modify inventory order because it is in '{order.InventoryOrderStatus.ToString()}' state.");
                }
            }

            return result;
        }
    }
}
