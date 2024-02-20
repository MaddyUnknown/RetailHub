using Dapper;
using Inventory.Core.Entities;
using Inventory.Core.Repository;
using Inventory.Infrastructure.Extensions;
using Inventory.Infrastructure.Factories.Interface;
using Microsoft.Extensions.Logging;
using System.Data;
using UserContext.Core.Interface;

namespace Inventory.Infrastructure.Repositories
{
    public class InventoryOrderRepository : IAsyncRepository<InventoryOrder>
    {
        private readonly string DATA_SOURCE = "inventory";

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;
        private readonly ILogger<InventoryOrderRepository> _logger;

        #region Constructor
        public InventoryOrderRepository(ISqlConnectionFactory connectionFactory, ILogger<InventoryOrderRepository> logger, IUserContext userContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Query Methods

        public async Task<IEnumerable<InventoryOrder>> GetAll()
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive all inventory orders: {ProcedureName}", "usp_GetAllInventoryOrder");
                var orders = await conn.QueryAsync<InventoryOrder>("usp_GetAllInventoryOrder", commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Retrieved {Count} inventory orders", orders.Count());
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all inventory orders");
                throw;
            }

        }

        public async Task<InventoryOrder?> GetByIdAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive inventory order by id: {ProcedureName}, Id: {OrderId}", "usp_GetInventoryOrderById", id);
                var orders = await conn.QueryAsync<InventoryOrder>("usp_GetInventoryOrderById", new { Id = id }, commandType: CommandType.StoredProcedure);
                var order = orders.FirstOrDefault();

                if (order == null)
                    _logger.LogInformation("No inventory order found for id: {OrderId}", id);
                else
                    _logger.LogInformation("Inventory order found for id: {OrderId}", id);

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching inventory order with id: {OrderId}", id);
                throw;
            }

        }

        #endregion

        #region Create, Update, Delete Methods

        public async Task<bool> AddAsync(InventoryOrder entity)
        {
            try
            {
                entity.SetCreatedBy(_userContext.UserName);
                entity.SetCreationDate(DateTime.Now);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("InventoryOrderNumber", entity.InventoryOrderNumber, DbType.String, ParameterDirection.Input);
                parameters.Add("ExternalOrderReferenceNumber", entity.ExternalOrderReferenceNumber, DbType.String, ParameterDirection.Input);
                parameters.Add("VendorId", entity.VendorId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("SubTotal", entity.SubTotal, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("Discount", entity.Discount, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("ShippingFee", entity.ShippingFee, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("TotalCost", entity.TotalCost, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("InventoryOrderStatus", entity.InventoryOrderStatus, DbType.Int32, ParameterDirection.Input);
                parameters.Add("CreatedBy", entity.CreatedBy, DbType.String, ParameterDirection.Input);
                parameters.Add("CreationDate", entity.CreationDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to add invetory order:  {ProcedureName}", "usp_InsertInventoryOrder");
                int rowsAffected = await conn.ExecuteAsync("usp_InsertInventoryOrder", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    entity.SetId(parameters.Get<long>("Id"));
                    _logger.LogInformation("Inventory order added with id: {OrderId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                }
                else
                {
                    _logger.LogWarning("No inventory order added");
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding inventory order");
                throw;
            }

        }

        public async Task<bool> UpdateAsync(InventoryOrder entity)
        {
            try
            {
                entity.SetUpdateDate(DateTime.Now);
                entity.SetUpdatedBy(_userContext.UserName);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("InventoryOrderNumber", entity.InventoryOrderNumber, DbType.String, ParameterDirection.Input);
                parameters.Add("ExternalOrderReferenceNumber", entity.ExternalOrderReferenceNumber, DbType.String, ParameterDirection.Input);
                parameters.Add("VendorId", entity.VendorId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("SubTotal", entity.SubTotal, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("Discount", entity.Discount, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("ShippingFee", entity.ShippingFee, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("TotalCost", entity.TotalCost, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("InventoryOrderStatus", entity.InventoryOrderStatus, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UpdatedBy", entity.UpdatedBy, DbType.String, ParameterDirection.Input);
                parameters.Add("UpdateDate", entity.UpdateDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to update inventory order: {ProcedureName}", "usp_UpdateInventoryOrder");
                int rowsAffected = await conn.ExecuteAsync("usp_UpdateInventoryOrder", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Updated inventory order with id: {OrderId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory order with id: {OrderId}", entity.Id);
                throw;
            }

        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to delete inventory order: {ProcedureName}", "usp_DeleteInventoryOrder");
                int rowsAffected = await conn.ExecuteAsync("usp_DeleteInventoryOrder", new { Id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Deleted inventory order with id: {OrderId}, RowsAffected: {RowsAffected}", id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting inventory order with id: {OrderId}", id);
                throw;
            }

        }

        #endregion

    }
}