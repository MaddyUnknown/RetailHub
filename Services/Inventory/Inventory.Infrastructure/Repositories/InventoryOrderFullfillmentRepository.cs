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
    public class InventoryOrderFullfillmentRepository : IAsyncRepository<InventoryOrderFullfillment>
    {
        private readonly string DATA_SOURCE = "inventory";

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;
        private readonly ILogger<InventoryOrderFullfillmentRepository> _logger;

        #region Constructor
        public InventoryOrderFullfillmentRepository(ISqlConnectionFactory connectionFactory, ILogger<InventoryOrderFullfillmentRepository> logger, IUserContext userContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Query Methods
        public async Task<IEnumerable<InventoryOrderFullfillment>> GetAll()
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive all inventory order fulfillments: {ProcedureName}", "dbo.usp_GetAllInventoryOrderFullfillment");
                var fulfillments = await conn.QueryAsync<InventoryOrderFullfillment>("dbo.usp_GetAllInventoryOrderFullfillment", commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Retrieved {Count} inventory order fulfillments", fulfillments.Count());
                return fulfillments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all inventory order fulfillments");
                throw;
            }

        }

        public async Task<InventoryOrderFullfillment?> GetByIdAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive inventory order fulfillment by id: {ProcedureName}, Id: {FulfillmentId}", "dbo.usp_GetInventoryOrderFullfillmentById", id);
                var fulfillments = await conn.QueryAsync<InventoryOrderFullfillment>("dbo.usp_GetInventoryOrderFullfillmentById", new { Id = id }, commandType: CommandType.StoredProcedure);
                var fulfillment = fulfillments.FirstOrDefault();

                if (fulfillment == null)
                    _logger.LogInformation("No inventory order fulfillment found for id: {FulfillmentId}", id);
                else
                    _logger.LogInformation("Inventory order fulfillment found for id: {FulfillmentId}", id);

                return fulfillment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching inventory order fulfillment with id: {FulfillmentId}", id);
                throw;
            }

        }
        #endregion

        #region Create, Update, Delete Methods
        public async Task<bool> AddAsync(InventoryOrderFullfillment entity)
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
                parameters.Add("ReceivedTimeStamp", entity.ReceivedTimeStamp, DbType.DateTime2, ParameterDirection.Input);
                parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("CreationDate", entity.CreationDate, DbType.DateTime2, ParameterDirection.Input);


                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to add inventory order fullfillment:  {ProcedureName}", "dbo.usp_InsertInventoryOrderFullfillment");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_InsertInventoryOrderFullfillment", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    entity.SetId(parameters.Get<long>("Id"));
                    _logger.LogInformation("Inventory order fulfillment added with id: {FulfillmentId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                }
                else
                {
                    _logger.LogWarning("No inventory order fulfillment added");
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding inventory order fulfillment");
                throw;
            }

        }

        public async Task<bool> UpdateAsync(InventoryOrderFullfillment entity)
        {
            try
            {
                entity.SetUpdateDate(DateTime.Now);
                entity.SetUpdatedBy(_userContext.UserName);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("InventoryOrderId", entity.InventoryOrderId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("ProductId", entity.ProductId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("Quantity", entity.Quantity, DbType.Int32, ParameterDirection.Input);
                parameters.Add("ReceivedTimeStamp", entity.ReceivedTimeStamp, DbType.DateTime2, ParameterDirection.Input);
                parameters.Add("UpdatedBy", entity.UpdatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UpdateDate", entity.UpdateDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to update inventory order fulfillment: {ProcedureName}", "dbo.usp_UpdateInventoryOrderFullfillment");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_UpdateInventoryOrderFullfillment", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Updated inventory order fulfillment with id: {FulfillmentId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory order fulfillment with id: {FulfillmentId}", entity.Id);
                throw;
            }

        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to delete inventory order fulfillment: {ProcedureName}", "dbo.usp_DeleteInventoryOrderFullfillment");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_DeleteInventoryOrderFullfillment", new { Id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Deleted inventory order fulfillment with id: {FulfillmentId}, RowsAffected: {RowsAffected}", id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting inventory order fulfillment with id: {FulfillmentId}", id);
                throw;
            }

        }
        #endregion

    }
}