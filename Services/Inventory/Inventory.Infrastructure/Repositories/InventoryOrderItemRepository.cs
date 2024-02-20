using Dapper;
using Inventory.Core.Entities;
using Inventory.Core.Repository;
using Inventory.Infrastructure.Extensions;
using Inventory.Infrastructure.Factories.Interface;
using Inventory.Infrastructure.Utility;
using Microsoft.Extensions.Logging;
using System.Data;
using UserContext.Core.Interface;

namespace Inventory.Infrastructure.Repositories
{
    public class InventoryOrderItemRepository : IInventoryOrderItemRepository
    {
        private readonly string DATA_SOURCE = "inventory";

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;
        private readonly ILogger<InventoryOrderItemRepository> _logger;

        #region Constructor
        public InventoryOrderItemRepository(ISqlConnectionFactory connectionFactory, ILogger<InventoryOrderItemRepository> logger, IUserContext userContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Query Methods
        public async Task<IEnumerable<InventoryOrderItem>> GetAll()
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive all inventory order items: {ProcedureName}", "usp_GetAllInventoryOrderItems");
                var items = await conn.QueryAsync<InventoryOrderItem>("usp_GetAllInventoryOrderItems", commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Retrieved {Count} inventory order items", items.Count());
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all inventory order items");
                throw;
            }

        }

        public async Task<InventoryOrderItem?> GetByIdAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive inventory order item by id: {ProcedureName}, Id: {ItemId}", "usp_GetInventoryOrderItemById", id);
                var items = await conn.QueryAsync<InventoryOrderItem>("usp_GetInventoryOrderItemById", new { Id = id }, commandType: CommandType.StoredProcedure);
                var item = items.FirstOrDefault();

                if (item == null)
                    _logger.LogInformation("No inventory order item found for id: {ItemId}", id);
                else
                    _logger.LogInformation("Inventory order item found for id: {ItemId}", id);

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching inventory order item with id: {ItemId}", id);
                throw;
            }

        }

        public async Task<IEnumerable<InventoryOrderItem>> GetByIdListAsync(IEnumerable<long> ids)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive inventory order item by id(s) : {ProcedureName}", "usp_GetInventoryOrderItemByIdList");
                var items = await conn.QueryAsync<InventoryOrderItem>("usp_GetInventoryOrderItemtByIdList", new { IdList = SqlDataTableUtil.CreateLongDataTable(ids) }, commandType: CommandType.StoredProcedure);

                if (items == null || items.Count() == 0)
                {
                    _logger.LogInformation("No inventory order item found for input id(s)");
                }
                else
                {
                    _logger.LogInformation("{FetchCount} inventory order items found for {InputCount} id(s)", items.Count(), ids.Count());
                }

                return items ?? Enumerable.Empty<InventoryOrderItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching inventory order items for input id(s)");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryOrderItem>> GetByOrderIdAsync(long orderId)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive invetory order items by inventory order id : {ProcedureName}", "usp_GetInventoryOrderItemByInventoryOrderId");
                var items = await conn.QueryAsync<InventoryOrderItem>("usp_GetInventoryOrderItemByInventoryOrderId", new { InventoryOrderId = orderId }, commandType: CommandType.StoredProcedure);

                if (items == null || items.Count() == 0)
                {
                    _logger.LogInformation("No inventory order item found for inventory order id: {InventoryOrderId}", orderId);
                }
                else
                {
                    _logger.LogInformation("Retrieved {Count} inventory order items for inventory order id : {InventoryOrderId}", items.Count(), orderId);
                }

                return items ?? Enumerable.Empty<InventoryOrderItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching inventory order items for inventory order id : {InvnetoryOrderId}", orderId);
                throw;
            }
        }
        #endregion

        #region Create, Update, Delete Methods
        public async Task<bool> AddAsync(InventoryOrderItem entity)
        {
            try
            {
                entity.SetCreatedBy(_userContext.UserName);
                entity.SetCreationDate(DateTime.Now);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("InventoryOrderId", entity.InventoryOrderId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("ProductId", entity.ProductId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("Quantity", entity.Quantity, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UnitPrice", entity.UnitPrice, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("CreatedBy", entity.CreatedBy, DbType.String, ParameterDirection.Input);
                parameters.Add("CreationDate", entity.CreationDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to add inventory order item:  {ProcedureName}", "usp_InsertInventoryOrderItem");
                int rowsAffected = await conn.ExecuteAsync("usp_InsertInventoryOrderItem", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    entity.SetId(parameters.Get<long>("Id"));
                    _logger.LogInformation("Inventory order item added with id: {ItemId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                }
                else
                {
                    _logger.LogWarning("No inventory order item added");
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding inventory order item");
                throw;
            }

        }

        public async Task<bool> UpdateAsync(InventoryOrderItem entity)
        {
            try
            {
                entity.SetUpdatedBy(_userContext.UserName);
                entity.SetUpdateDate(DateTime.Now);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("InventoryOrderId", entity.InventoryOrderId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("ProductId", entity.ProductId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("Quantity", entity.Quantity, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UnitPrice", entity.UnitPrice, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("UpdatedBy", entity.UpdatedBy, DbType.String, ParameterDirection.Input);
                parameters.Add("UpdateDate", entity.UpdateDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to update inventory order item: {ProcedureName}", "usp_UpdateInventoryOrderItem");
                int rowsAffected = await conn.ExecuteAsync("usp_UpdateInventoryOrderItem", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Updated inventory order item with id: {ItemId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory order item with Id: {ItemId}", entity.Id);
                throw;
            }

        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to update inventory order item: {ProcedureName}", "usp_DeleteInventoryOrderItem");
                int rowsAffected = await conn.ExecuteAsync("usp_DeleteInventoryOrderItem", new { Id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Deleted inventory order item with id: {ItemId}, RowsAffected: {RowsAffected}", id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting inventory order item with id: {ItemId}", id);
                throw;
            }

        }
        #endregion

    }
}