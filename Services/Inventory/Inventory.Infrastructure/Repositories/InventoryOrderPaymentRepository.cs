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
    public class InventoryOrderPaymentRepository : IAsyncRepository<InventoryOrderPayment>
    {
        private readonly string DATA_SOURCE = "inventory";

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;
        private readonly ILogger<InventoryOrderPaymentRepository> _logger;

        #region Constructor
        public InventoryOrderPaymentRepository(ISqlConnectionFactory connectionFactory, ILogger<InventoryOrderPaymentRepository> logger, IUserContext userContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Query Methods
        public async Task<IEnumerable<InventoryOrderPayment>> GetAll()
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive all inventory order payments: {ProcedureName}", "dbo.usp_GetAllInventoryOrderPayments");
                var payments = await conn.QueryAsync<InventoryOrderPayment>("dbo.usp_GetAllInventoryOrderPayments", commandType: CommandType.StoredProcedure);

                _logger.LogInformation( "Retrieved {Count} inventory order payments", payments.Count());
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all inventory order payments");
                throw;
            }
        }

        public async Task<InventoryOrderPayment?> GetByIdAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive inventory order payment by id: {ProcedureName}, Id: {PaymentId}", "dbo.usp_GetInventoryOrderPaymentById", id);
                var payments = await conn.QueryAsync<InventoryOrderPayment>("dbo.usp_GetInventoryOrderPaymentById", new { Id = id }, commandType: CommandType.StoredProcedure);
                var payment = payments.FirstOrDefault();

                if (payment == null)
                    _logger.LogInformation("No inventory order payment found for id: {PaymentId}", id);
                else
                    _logger.LogInformation("Inventory order payment found for id: {PaymentId}", id);

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching inventory order payment with id: {PaymentId}", id);
                throw;
            }
        }
        #endregion

        #region Create, Update, Delete Methods
        public async Task<bool> AddAsync(InventoryOrderPayment entity)
        {
            try
            {
                entity.SetCreatedBy(_userContext.UserName);
                entity.SetCreationDate(DateTime.Now);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("InventoryOrderId", entity.InventoryOrderId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("PaymentTimeStamp", entity.PaymentTimeStamp, DbType.DateTime2, ParameterDirection.Input);
                parameters.Add("PaymentAmount", entity.PaymentAmount, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("PaymentType", entity.PaymentType, DbType.Int32, ParameterDirection.Input);
                parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("CreationDate", entity.CreationDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to add inventory order payment: {ProcedureName}", "dbo.usp_InsertInventoryOrderPayment");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_InsertInventoryOrderPayment", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    entity.SetId(parameters.Get<long>("Id"));
                    _logger.LogInformation("Inventory order payment added with Id: {PaymentId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                }
                else
                {
                    _logger.LogWarning("No inventory order payment added");
                }

                return rowsAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding inventory order payment");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(InventoryOrderPayment entity)
        {
            try
            {
                entity.SetUpdateDate(DateTime.Now);
                entity.SetUpdatedBy(_userContext.UserName);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("InventoryOrderId", entity.InventoryOrderId, DbType.Int64, ParameterDirection.Input);
                parameters.Add("PaymentTimeStamp", entity.PaymentTimeStamp, DbType.DateTime2, ParameterDirection.Input);
                parameters.Add("PaymentAmount", entity.PaymentAmount, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("PaymentType", entity.PaymentType, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UpdatedBy", entity.UpdatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UpdateDate", entity.UpdateDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to update inventory order payment: {ProcedureName}", "dbo.usp_UpdateInventoryOrderPayment");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_UpdateInventoryOrderPayment", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Updated inventory order payment with id: {PaymentId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                return rowsAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating inventory order payment with id: {PaymentId}", entity.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation( "Executing stored procedure to delete inventory order payment: {ProcedureName}", "dbo.usp_DeleteInventoryOrderPayment");
                int rowsAffected = await conn.ExecuteAsync("dbo.usp_DeleteInventoryOrderPayment", new { Id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Deleted inventory order payment with id: {PaymentId}, RowsAffected: {RowsAffected}", id, rowsAffected);
                return rowsAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting inventory order payment with id: {PaymentId}", id);
                throw;
            }
        }
        #endregion
    }
}