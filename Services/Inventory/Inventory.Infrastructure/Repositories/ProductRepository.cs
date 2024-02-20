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
    public class ProductRepository : IProductRepository
    {
        private readonly string DATA_SOURCE = "inventory";

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;
        private readonly ILogger<ProductRepository> _logger;

        #region Constructor
        public ProductRepository(ISqlConnectionFactory connectionFactory, ILogger<ProductRepository> logger, IUserContext userContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Query Methods
        public async Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive all products: {ProcedureName}", "usp_GetAllProduct");
                var products = await conn.QueryAsync<Product>("usp_GetAllProduct", commandType: CommandType.StoredProcedure);
                
                _logger.LogInformation("Retrieved {Count} products", products.Count());
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products");
                throw;
            }
            
        }

        public async Task<Product?> GetByIdAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive product by id: {ProcedureName}, Id: {ProductId}", "usp_GetProductById", id);
                var products = await conn.QueryAsync<Product>("usp_GetProductById", new { Id = id }, commandType: CommandType.StoredProcedure);
                var product = products.FirstOrDefault();
                
                if(product==null)
                    _logger.LogInformation("No product found for id: {ProductId}", id);
                else
                    _logger.LogInformation("Product found for id: {ProductId}", id);

                return product;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with id: {ProductId}", id);
                throw;
            }
            
        }


        public async Task<IEnumerable<Product>> GetByIdListAsync(IEnumerable<long> ids)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to retrive product by id(s) : {ProcedureName}", "usp_GetProductByIdList");
                var products = await conn.QueryAsync<Product>("usp_GetProductByIdList", new { IdList = SqlDataTableUtil.CreateLongDataTable(ids) }, commandType: CommandType.StoredProcedure);

                if(products == null || products.Count() == 0)
                {
                    _logger.LogInformation("No product found for input id(s)");
                }
                else
                {
                    _logger.LogInformation("{FetchCount} product found for {InputCount} id(s)", products.Count(), ids.Count());
                }

                return products ?? new List<Product>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching products for input id(s)");
                throw;
            }
        }
        #endregion

        #region Create, Update, Delete Methods
        public async Task<bool> AddAsync(Product entity)
        {
            try
            {
                entity.SetCreatedBy(_userContext.UserName);
                entity.SetCreationDate(DateTime.Now);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("ProductCode", entity.ProductCode, DbType.String, ParameterDirection.Input);
                parameters.Add("ProductName", entity.ProductName, DbType.String, ParameterDirection.Input);
                parameters.Add("ProductDescription", entity.ProductDescription, DbType.String, ParameterDirection.Input);
                parameters.Add("Price", entity.Price, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("Quantity", entity.Quantity, DbType.Int32, ParameterDirection.Input);
                parameters.Add("CreatedBy", entity.CreatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("CreationDate", entity.CreationDate, DbType.DateTime2, ParameterDirection.Input);


                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to add product:  {ProcedureName}", "usp_InsertProduct");
                int rowsAffected = await conn.ExecuteAsync("usp_InsertProduct", parameters, commandType: CommandType.StoredProcedure);

                if (rowsAffected > 0)
                {
                    entity.SetId(parameters.Get<long>("Id"));
                    _logger.LogInformation("Product added with id: {ProductId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                }
                else
                {
                    _logger.LogWarning("No product added");
                }

                return rowsAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while adding product");
                throw;
            }
            
        }

        public async Task<bool> UpdateAsync(Product entity)
        {
            try
            {
                entity.SetUpdateDate(DateTime.Now);
                entity.SetUpdatedBy(_userContext.UserName);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("ProductCode", entity.ProductCode, DbType.String, ParameterDirection.Input);
                parameters.Add("ProductName", entity.ProductName, DbType.String, ParameterDirection.Input);
                parameters.Add("ProductDescription", entity.ProductDescription, DbType.String, ParameterDirection.Input);
                parameters.Add("Price", entity.Price, DbType.Decimal, ParameterDirection.Input);
                parameters.Add("Quantity", entity.Quantity, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UpdatedBy", entity.UpdatedBy, DbType.Int32, ParameterDirection.Input);
                parameters.Add("UpdateDate", entity.UpdateDate, DbType.DateTime2, ParameterDirection.Input);

                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure to update product: {ProcedureName}", "usp_UpdateProduct");
                int rowsAffected = await conn.ExecuteAsync("usp_UpdateProduct", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Updated product with id: {ProductId}, RowsAffected: {RowsAffected}", entity.Id, rowsAffected);
                return rowsAffected > 0 ? true : false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with id: {ProductId}", entity.Id);
                throw;
            }
            
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using IDbConnection conn = _connectionFactory.GetConnection(DATA_SOURCE);

                _logger.LogInformation("Executing stored procedure for deleting product: {ProcedureName}", "usp_DeleteProduct");
                int rowsAffected = await conn.ExecuteAsync("usp_DeleteProduct", new { Id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Deleted product with id: {ProductId}, RowsAffected: {RowsAffected}", id, rowsAffected);
                return rowsAffected > 0 ? true : false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with id: {ProductId}", id);
                throw;
            }
            
        }
        #endregion

    }
}