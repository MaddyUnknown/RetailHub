using System.Data;
using System.Data.SqlClient;
using Inventory.Infrastructure.Factories.Interface;
using Microsoft.Extensions.Configuration;

namespace Inventory.Infrastructure.Factories
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConfiguration _config;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IDbConnection GetConnection(string dbSource)
        {
            return new SqlConnection(_config.GetConnectionString(dbSource));
        }
    }
}