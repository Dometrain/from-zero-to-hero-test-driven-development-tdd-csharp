using System.Data;

namespace Pricing.Infrastructure;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}