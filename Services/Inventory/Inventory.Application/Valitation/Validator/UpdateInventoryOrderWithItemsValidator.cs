using Inventory.Application.Model.Request;
using Inventory.Application.Valitation.Interface;
using Inventory.Core.Entities;
using Inventory.Core.Enum;
using Inventory.Core.Repository;

namespace Inventory.Application.Valitation.Validator
{
    public class UpdateInventoryOrderWithItemsValidator : IValidator<UpdateInventoryOrderWithItems>
    {
        private IProductRepository _productRepo;
        private IAsyncRepository<Vendor> _vendorRepo;
        private IAsyncRepository<InventoryOrder> _inventoryOrderRepository;
        private IInventoryOrderItemRepository _inventoryOrderItemRepository;

        public UpdateInventoryOrderWithItemsValidator(IProductRepository productRepository, IAsyncRepository<Vendor> vendorRepository,
            IAsyncRepository<InventoryOrder> inventoryOrderRepository, IInventoryOrderItemRepository inventoryOrderItemRepository)
        {
            _productRepo = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _vendorRepo = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
            _inventoryOrderRepository = inventoryOrderRepository ?? throw new ArgumentNullException(nameof(inventoryOrderRepository));
            _inventoryOrderItemRepository = inventoryOrderItemRepository ?? throw new ArgumentNullException(nameof(inventoryOrderItemRepository));
        }

        public async Task<ValidationResult> IsValid(UpdateInventoryOrderWithItems obj)
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
                else if(order.InventoryOrderStatus != InventoryOrderStatusEnum.Created)
                {
                    result.Errors.Add($"Cannot modify inventory order because it is '{order.InventoryOrderStatus.ToString()}' state.");
                }
            }

            // Vendor Id Validation
            if (obj.VendorId == null || obj.VendorId < 1)
            {
                result.Errors.Add($"VendorId must be greater than or equal to 1.");
                result.IsValid = false;
            }
            else if (obj.VendorId != null)
            {
                var vendor = await _vendorRepo.GetByIdAsync(obj.VendorId.Value);
                if (vendor == null)
                {
                    result.Errors.Add($"Vendor with Id {obj.VendorId} does not exist.");
                    result.IsValid = false;
                }
            }

            // Discount Validation
            if (obj.Discount == null || obj.Discount < 0)
            {
                result.Errors.Add("Discount must be a non-negative value.");
                result.IsValid = false;
            }

            // Shipping Fee Validation
            if (obj.ShippingFee == null || obj.ShippingFee < 0)
            {
                result.Errors.Add("Shipping fee must be a non-negative value.");
                result.IsValid = false;
            }

            // Items Validation
            if (obj.Items == null || obj.Items.Count == 0)
            {
                result.Errors.Add("Items list cannot be null or empty.");
                result.IsValid = false;
            }
            else
            {
                var inputProductIds = obj.Items.Where(x => x.ProductId != null && x.ProductId >= 1).Select(x => x.ProductId!.Value).ToList();
                var productList = await _productRepo.GetByIdListAsync(inputProductIds);

                var inputOrderItemsIds = obj.Items.Where(x => x.Id != null && x.Id >= 1).Select(x => x.Id!.Value).ToList() ?? Enumerable.Empty<long>();
                var inventoryOrderItemList = Enumerable.Empty<InventoryOrderItem>();
                if (inputOrderItemsIds != null && inputOrderItemsIds.Count() != 0)
                {
                    inventoryOrderItemList = await _inventoryOrderItemRepository.GetByIdListAsync(inputOrderItemsIds);
                }

                foreach (var item in obj.Items)
                {
                    if (item.Id != null && item.Id < 1)
                    {
                        result.Errors.Add($"Item Id must either be null or greater than or equal to 1.");
                        result.IsValid = false;
                    }
                    else if (item.Id != null && inventoryOrderItemList.FirstOrDefault(x => x.Id == item.Id) == null)
                    {
                        result.Errors.Add($"Item with Id {item.Id} does not exist.");
                        result.IsValid = false;
                    }

                    if (item.ProductId == null || item.ProductId < 1)
                    {
                        result.Errors.Add($"ProductId must be greater than or equal to 1.");
                        result.IsValid = false;
                    }
                    else if (productList.FirstOrDefault(x => x.Id == item.ProductId) == null)
                    {
                        result.Errors.Add($"Product with Id {item.ProductId} does not exist.");
                        result.IsValid = false;
                    }

                    if (item.Quantity == null || item.Quantity < 1)
                    {
                        result.Errors.Add("Quantity must be a positive value.");
                        result.IsValid = false;
                    }

                    if (item.UnitPrice == null || item.UnitPrice < 0)
                    {
                        result.Errors.Add("Unit price must be a non-negative value.");
                        result.IsValid = false;
                    }
                }
            }

            return result;
        }

    }
}
