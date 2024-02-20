using System.Data;

namespace Inventory.Infrastructure.Factories.Interface
{
    public interface ISqlConnectionFactory : IConnectionFactory<IDbConnection, string>
    {

    }
}