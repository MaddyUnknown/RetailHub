using Inventory.Application.Constant;
using Inventory.Application.Exception;
using Inventory.Application.Mapper;
using Inventory.Application.Model.DTO.InventoryOrder;
using Inventory.Application.Model.Request;
using Inventory.Application.Service.Interface;
using Inventory.Application.Utils;
using Inventory.Application.Valitation;
using Inventory.Application.Valitation.Interface;
using Inventory.Core.Entities;
using Inventory.Core.Enum;
using Inventory.Core.Repository;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Inventory.Application.Service
{
    public class InventoryOrderService : IInventoryOrderService
    {
        #region Private fields
        private IValidatorService _validatorService;
        private IAsyncRepository<InventoryOrder> _invetoryOrderRepository;
        private IInventoryOrderItemRepository _invetoryOrderItemRepository;
        private ILogger<InventoryOrderService> _logger;
        #endregion

        #region Constructors
        public InventoryOrderService(IValidatorService validatorService,
            IAsyncRepository<InventoryOrder> inventoryOrderRepository,
            IInventoryOrderItemRepository inventoryOrderItemRepository,
            ILogger<InventoryOrderService> logger)
        {
            _validatorService = validatorService ?? throw new ArgumentNullException(nameof(validatorService));
            _invetoryOrderRepository = inventoryOrderRepository ?? throw new ArgumentNullException(nameof(inventoryOrderRepository));
            _invetoryOrderItemRepository = inventoryOrderItemRepository ?? throw new ArgumentNullException(nameof(inventoryOrderItemRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Public Methods
        public async Task<GetInventoryOrderWithItemsDTO> CreateInventoryOrder(CreateInventoryOrderWithItems model)
        {
            _logger.LogInformation("Creating inventory order started.");
            try
            {
                // Validate the model
                ValidationResult validationResult = await _validatorService.Validate(model);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors,"Request validation failed.");
                }

                // Begin transaction
                using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Generate a unique code for the inventory order
                    string uniqueCode = GuidUtil.GetUniqueId(IdentifierPrefix.INVENTORY_ORDER);

                    // Create inventory order entity
                    var inventoryOrderEntity = InventoryOrderMapper.CreateInventoryOrder(model);
                    inventoryOrderEntity.InventoryOrderNumber = uniqueCode;
                    inventoryOrderEntity.InventoryOrderStatus = Core.Enum.InventoryOrderStatusEnum.CREATED;

                    // Add inventory order to database
                    bool orderAddSuccess = await _invetoryOrderRepository.AddAsync(inventoryOrderEntity);
                    if (!orderAddSuccess)
                    {
                        throw new ServiceException("Error occurred while adding inventory order.");
                    }

                    // Create and add inventory order items
                    List<InventoryOrderItem> orderItemEntityList = new List<InventoryOrderItem>();
                    if (model.Items != null)
                    {
                        foreach (var item in model.Items)
                        {
                            var orderItemEntity = InventoryOrderItemMapper.CreateInventoryOrderItem(item, inventoryOrderEntity.Id);
                            bool itemAddSuccess = await _invetoryOrderItemRepository.AddAsync(orderItemEntity);
                            if (!itemAddSuccess)
                            {
                                throw new ServiceException("Error occurred while adding inventory order item.");
                            }
                            orderItemEntityList.Add(orderItemEntity);
                        }
                    }

                    // Complete transaction
                    ts.Complete();

                    // Return created inventory order with items
                    return InventoryOrderMapper.GetInventoryOrderWithItems(inventoryOrderEntity, orderItemEntityList);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating inventory order.");
                throw;
            }
            finally
            {
                _logger.LogInformation("Creating inventory order ended.");
            }
        }

        public async Task<GetInventoryOrderWithItemsDTO> UpdateInventoryOrder(UpdateInventoryOrderWithItems model)
        {
            _logger.LogInformation("Updating inventory order started.");
            try
            {
                // Validate the model
                ValidationResult validationResult = await _validatorService.Validate(model);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors, "Request validation failed.");
                }

                using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Fetch current order and items from the database
                    InventoryOrder inventoryOrderEntity = await _invetoryOrderRepository.GetByIdAsync(model.Id!.Value)
                        ?? throw new ServiceException($"Inventory order not found for Id {model.Id!.Value}.");

                    IEnumerable<InventoryOrderItem> currentOrderItemEntities = await _invetoryOrderItemRepository.GetByOrderIdAsync(inventoryOrderEntity.Id) ?? Enumerable.Empty<InventoryOrderItem>();

                    // Update the inventory order
                    InventoryOrder updatedOrderEntity = InventoryOrderMapper.UpdateInventoryOrder(inventoryOrderEntity, model);
                    bool orderUpdateSuccess = await _invetoryOrderRepository.UpdateAsync(updatedOrderEntity);
                    if (!orderUpdateSuccess)
                    {
                        throw new ServiceException("Error occurred while updating inventory order.");
                    }

                    // Update, insert, or delete order items
                    List<InventoryOrderItem> updatedOrderItemEntityList = new List<InventoryOrderItem>();

                    // Delete items that are not present in the model
                    var itemIdsInModel = model.Items?.Where(id => id != null).Select(x => x.Id!.Value).ToList() ?? Enumerable.Empty<long>();
                    var itemsToDelete = currentOrderItemEntities.Where(item => !itemIdsInModel.Contains(item.Id));
                    foreach (var itemToDelete in itemsToDelete)
                    {
                        bool deleteSuccess = await _invetoryOrderItemRepository.DeleteAsync(itemToDelete.Id);
                        if (!deleteSuccess)
                        {
                            throw new ServiceException("Error occured while deleting inventory order item.");
                        }
                    }

                    // Update or insert items from the model
                    foreach (var itemInput in model.Items ?? Enumerable.Empty<UpdateInventoryOrderItemForOrder>())
                    {
                        if (itemInput.Id == null)
                        {
                            // Add new order item
                            var newOrderItemEntity = InventoryOrderItemMapper.CreateInventoryOrderItem(itemInput, updatedOrderEntity.Id);
                            bool addItemSuccess = await _invetoryOrderItemRepository.AddAsync(newOrderItemEntity);
                            if (!addItemSuccess)
                            {
                                throw new ServiceException("Error occurred while adding inventory order item.");
                            }
                            updatedOrderItemEntityList.Add(newOrderItemEntity);
                        }
                        else
                        {
                            var existingOrderItem = currentOrderItemEntities.FirstOrDefault(x => x.Id == itemInput.Id) ??
                                throw new ServiceException($"Item with Id {itemInput.Id} not found in the current order items.");

                            // Update existing order item
                            InventoryOrderItem updatedOrderItemEntity = InventoryOrderItemMapper.UpdateInventoryOrderItem(existingOrderItem, itemInput, updatedOrderEntity.Id);
                            bool updateItemSuccess = await _invetoryOrderItemRepository.UpdateAsync(updatedOrderItemEntity);
                            if (!updateItemSuccess)
                            {
                                throw new ServiceException("Error occured while updating inventory order item.");
                            }
                            updatedOrderItemEntityList.Add(updatedOrderItemEntity);
                        }
                    }

                    // Complete the transaction
                    ts.Complete();

                    // Return the updated order with items
                    return InventoryOrderMapper.GetInventoryOrderWithItems(updatedOrderEntity, updatedOrderItemEntityList);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory order.");
                throw;
            }
            finally
            {
                _logger.LogInformation("Updating inventory order ended.");
            }
        }

        public async Task<GetInventoryOrderWithItemsDTO> CancelInventoryOrder(CancelInventoryOrder model)
        {
            _logger.LogInformation("Cancel inventory order started.");
            try
            {
                // Validate the model
                ValidationResult validationResult = await _validatorService.Validate(model);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors, "Request validation failed.");
                }

                using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Fetch current order and items from the database
                    InventoryOrder inventoryOrderEntity = await _invetoryOrderRepository.GetByIdAsync(model.Id!.Value)
                        ?? throw new ServiceException($"Inventory order not found for Id {model.Id!.Value}.");

                    IEnumerable<InventoryOrderItem> currentOrderItemEntities = await _invetoryOrderItemRepository.GetByOrderIdAsync(inventoryOrderEntity.Id) ?? Enumerable.Empty<InventoryOrderItem>();

                    // Update the inventory order
                    inventoryOrderEntity.InventoryOrderStatus = InventoryOrderStatusEnum.CANCELLED;
                    bool orderUpdateSuccess = await _invetoryOrderRepository.UpdateAsync(inventoryOrderEntity);
                    if (!orderUpdateSuccess)
                    {
                        throw new ServiceException("Error occurred while cancelling inventory order.");
                    }

                    // Complete the transaction
                    ts.Complete();

                    // Return the updated order with items
                    return InventoryOrderMapper.GetInventoryOrderWithItems(inventoryOrderEntity, currentOrderItemEntities);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cancelling inventory order.");
                throw;
            }
            finally
            {
                _logger.LogInformation("Cancel inventory order ended.");
            }
        }

        public async Task<GetInventoryOrderWithItemsDTO> GetInventoryOrderById(GetInventoryOrderWithItemsById model)
        {
            _logger.LogInformation("Get inventory order by id started.");
            try
            {
                // Validate the model
                ValidationResult validationResult = await _validatorService.Validate(model);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors, "Request validation failed.");
                }

                using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Fetch current order and items from the database
                    InventoryOrder inventoryOrderEntity = await _invetoryOrderRepository.GetByIdAsync(model.Id!.Value)
                        ?? throw new ServiceException($"Inventory order not found for Id {model.Id!.Value}.");

                    IEnumerable<InventoryOrderItem> currentOrderItemEntities = await _invetoryOrderItemRepository.GetByOrderIdAsync(inventoryOrderEntity.Id) ?? Enumerable.Empty<InventoryOrderItem>();

                    // Complete the transaction
                    ts.Complete();

                    // Return the updated order with items
                    return InventoryOrderMapper.GetInventoryOrderWithItems(inventoryOrderEntity, currentOrderItemEntities);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting inventory order by id.");
                throw;
            }
            finally
            {
                _logger.LogInformation("Get inventory order by id ended.");
            }
        }
        #endregion
    }
}