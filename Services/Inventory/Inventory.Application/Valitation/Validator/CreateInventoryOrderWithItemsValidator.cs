using Inventory.Application.Model.Request;
using Inventory.Application.Valitation.Interface;
using Inventory.Core.Entities;
using Inventory.Core.Repository;

namespace Inventory.Application.Valitation.Validator
{
    public class CreateInventoryOrderWithItemsValidator : IValidator<CreateInventoryOrderWithItems>
    {
        private IProductRepository _productRepo;
        private IAsyncRepository<Vendor> _vendorRepo;

        public CreateInventoryOrderWithItemsValidator(IProductRepository productRepository, IAsyncRepository<Vendor> vendorRepository)
        {
            _productRepo = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _vendorRepo = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        }

        public async Task<ValidationResult> IsValid(CreateInventoryOrderWithItems obj)
        {
            ValidationResult result = new ValidationResult();
            result.IsValid = true;

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

                foreach (var item in obj.Items)
                {
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