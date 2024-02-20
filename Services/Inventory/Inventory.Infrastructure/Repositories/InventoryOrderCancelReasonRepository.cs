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
    public class InventoryOrderCancelReasonRepository : IAsyncRepository<InventoryOrderCancelReason>
    {
        private readonly string DATA_SOURCE = "inventory";

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;
        private readonly ILogger<InventoryOrderCancelReasonRepository> _logger;

        #region Constructor
        public InventoryOrderCancelReasonRepository(ISqlConnectionFactory connectionFactory, ILogger<InventoryOrderCancelReasonRepository> logger, IUserContext userContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Query Methods
        public async Task<IEnumerable<InventoryOrderCancelReason>> GetAll()
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive all inventory order cancel reasons: {ProcedureName}", "dbo.usp_GetAllInventoryOrderCancelReasons");
                var cancelReasons = await conn.QueryAsync<InventoryOrderCancelReason>("dbo.usp_GetAllInventoryOrderCancelReasons", commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Retrieved {Count} inventory order cancel reasons", cancelReasons.Count());
                return cancelReasons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all inventory order cancel reasons");
                throw;
            }
        }

        public async Task<InventoryOrderCancelReason?> GetByIdAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive inventory order cancel reason by id: {ProcedureName}, Id: {CancelReasonId}", "dbo.usp_GetInventoryOrderCancelReasonById", id);
                var cancelReasons = await conn.QueryAsync<InventoryOrderCancelReason>("dbo.usp_GetInventoryOrderCancelReasonById", new { Id = id }, commandType: CommandType.StoredProcedure);
                var cancelReason = cancelReasons.FirstOrDefault();

                if (cancelReason == null)
                    _logger.LogInformation("No inventory order cancel reason found for id: {CancelReasonId}", id);
                else
                    _logger.LogInformation("Inventory order cancel reason found for id: {CancelReasonId}", id);

                return cancelReason;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching inventory order cancel reason with id: {CancelReasonId}", id);
                throw;
            }
        }
        #endregion

        #region Create, Update, Delete Methods
        public async Task<bool> AddAsync(InventoryOrderCancelReason entity)
        {
            try
            {
                entity.SetCreatedBy(_userContext.UserName);
                entity.SetCreationDate(DateTime.Now);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("InventoryOrderId", entity.InventoryOrderId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("OrderCancelReason", entity.OrderCancelReason, DbType.String, ParameterDirection.Input);
                parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("CreationDate", entity.CreationDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to add inventory order cancel reason: {ProcedureName}", "dbo.usp_InsertInventoryOrderCancelReason");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_InsertInventoryOrderCancelReason", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    entity.SetId(parameters.Get<long>("Id"));
                    _logger.LogInformation("Inventory order cancel reason added with id: {CancelReasonId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                }
                else
                {
                    _logger.LogWarning("No inventory order cancel reason added");
                }

                return rowsAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding inventory order cancel reason");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(InventoryOrderCancelReason entity)
        {
            try
            {
                entity.SetUpdateDate(DateTime.Now);
                entity.SetUpdatedBy(_userContext.UserName);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("InventoryOrderId", entity.InventoryOrderId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("OrderCancelReason", entity.OrderCancelReason, DbType.String, ParameterDirection.Input);
                parameters.Add("UpdatedBy", entity.UpdatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UpdateDate", entity.UpdateDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to update inventory order cancel reason: {ProcedureName}", "dbo.usp_UpdateInventoryOrderCancelReason");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_UpdateInventoryOrderCancelReason", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Updated inventory order cancel reason with id: {CancelReasonId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                return rowsAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory order cancel reason with id: {CancelReasonId}", entity.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to delete inventory order cancel reason: {ProcedureName}", "dbo.usp_DeleteInventoryOrderCancelReason");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_DeleteInventoryOrderCancelReason", new { Id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Deleted inventory order cancel reason with id: {CancelReasonId}, RowsAffected: {RowsAffected}", id, rowsAffected);
                return rowsAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting inventory order cancel reason with id: {CancelReasonId}", id);
                throw;
            }
        }
        #endregion
    }
}