using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using UserContext.Core.Interface;
using Inventory.Core.Entities;
using Inventory.Core.Repository;
using Inventory.Infrastructure.Extensions;
using Inventory.Infrastructure.Factories.Interface;

namespace Inventory.Infrastructure.Repositories
{
    public class VendorRepository : IAsyncRepository<Vendor>
    {
        private readonly string DATA_SOURCE = "inventory";

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;
        private readonly ILogger<VendorRepository> _logger;

        #region Constructor
        public VendorRepository(ISqlConnectionFactory connectionFactory, ILogger<VendorRepository> logger, IUserContext userContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Query Methods
        public async Task<IEnumerable<Vendor>> GetAll()
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure for retrieving all vendors: {ProcedureName}", "usp_GetAllVendors");
                var vendors = await conn.QueryAsync<Vendor>("usp_GetAllVendors", commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Retrieved {Count} vendors", vendors.Count());
                return vendors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all vendors");
                throw;
            }
        }

        public async Task<Vendor?> GetByIdAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure for retrieving vendor by Id: {ProcedureName}, Id: {VendorId}", "usp_GetVendorById", id);
                var vendors = await conn.QueryAsync<Vendor>("usp_GetVendorById", new { Id = id }, commandType: CommandType.StoredProcedure);
                var vendor = vendors.FirstOrDefault();

                if (vendor == null)
                    _logger.LogInformation("No vendor found for Vendor Id: {VendorId}", id);
                else
                    _logger.LogInformation("Vendor found for Vendor Id: {VendorId}", id);

                return vendor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching vendor with Id: {VendorId}", id);
                throw;
            }
        }
        #endregion

        #region Create, Update, Delete Methods
        public async Task<bool> AddAsync(Vendor entity)
        {
            try
            {
                entity.SetCreatedBy(_userContext.UserName);
                entity.SetCreationDate(DateTime.Now);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("VendorCode", entity.VendorCode, DbType.String, ParameterDirection.Input);
                parameters.Add("VendorName", entity.VendorName, DbType.String, ParameterDirection.Input);
                parameters.Add("VendorAddress", entity.VendorAddress, DbType.String, ParameterDirection.Input);
                parameters.Add("CreatedBy", entity.CreatedBy, DbType.String, ParameterDirection.Input);
                parameters.Add("CreationDate", entity.CreationDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure for adding vendor:  {ProcedureName}", "usp_InsertVendor");
                int rowsAffected = await conn.ExecuteAsync("usp_InsertVendor", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    entity.SetId(parameters.Get<long>("Id"));
                    _logger.LogInformation("Vendor added with Id: {VendorId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                }
                else
                {
                    _logger.LogWarning("No vendor added");
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding vendor");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Vendor entity)
        {
            try
            {
                entity.SetUpdateDate(DateTime.Now);
                entity.SetUpdatedBy(_userContext.UserName);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("VendorCode", entity.VendorCode, DbType.String, ParameterDirection.Input);
                parameters.Add("VendorName", entity.VendorName, DbType.String, ParameterDirection.Input);
                parameters.Add("VendorAddress", entity.VendorAddress, DbType.String, ParameterDirection.Input);
                parameters.Add("UpdatedBy", entity.UpdatedBy, DbType.String, ParameterDirection.Input);
                parameters.Add("UpdateDate", entity.UpdateDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure for updating vendor: {ProcedureName}", "usp_UpdateVendor");
                int rowsAffected = await conn.ExecuteAsync("usp_UpdateVendor", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Updated vendor with Id: {VendorId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating vendor with Id: {VendorId}", entity.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure for deleting vendor: {ProcedureName}", "usp_DeleteVendor");
                int rowsAffected = await conn.ExecuteAsync("usp_DeleteVendor", new { Id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Deleted vendor with Id: {VendorId}, RowsAffected: {RowsAffected}", id, rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting vendor with Id: {VendorId}", id);
                throw;
            }
        }
        #endregion
    }
}