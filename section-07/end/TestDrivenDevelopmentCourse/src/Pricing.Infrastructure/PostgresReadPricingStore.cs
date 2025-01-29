using Dapper;
using Newtonsoft.Json;
using Pricing.Core.Domain;
using Pricing.Core.TicketPrice;

namespace Pricing.Infrastructure;

public class PostgresReadPricingStore: IReadPricingStore
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PostgresReadPricingStore(IDbConnectionFactory dbConnectionFactory)
    {
        ArgumentNullException.ThrowIfNull(dbConnectionFactory);
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PricingTable> GetAsync(CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryFirstAsync<string>(
            @"SELECT document FROM Pricing;");

        return JsonConvert.DeserializeObject<PricingTable>(result)!;
    }
}