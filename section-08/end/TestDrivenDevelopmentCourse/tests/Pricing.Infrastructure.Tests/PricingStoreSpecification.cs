using System.Text.Json;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Pricing.Core;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;

namespace Pricing.Infrastructure.Tests;

public class PricingStoreSpecification : IClassFixture<PostgreSqlFixture>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PricingStoreSpecification(PostgreSqlFixture fixture)
    {
        _dbConnectionFactory = fixture.ConnectionFactory;
    }

    [Fact]
    public void Should_throw_argument_null_exception_if_missing_connection_string()
    {
        var create = () => new PostgresPricingStore(null!);

        create.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task Should_return_true_when_save_with_success()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        var pricingTable = CreatePricingTable();

        var result = await store.SaveAsync(pricingTable, default);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_insert_if_not_exists()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        var pricingTable = CreatePricingTable();
        await CleanUpPricingStore();

        var result = await store.SaveAsync(pricingTable, default);

        result.Should().BeTrue();
    }


    [Fact]
    public async Task Should_replace_if_already_exists()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        await store.SaveAsync(CreatePricingTable(), default);
        var newPricingTable = new PricingTable(new[]
        {
            new PriceTier(24, 21)
        });

        var result = await store.SaveAsync(newPricingTable, default);

        using (new AssertionScope())
        {
            result.Should().BeTrue();
            var data = await GetPricingFromStore();
            data.Should().HaveCount(1)
                .And
                .Subject
                .First().document.Equals(JsonSerializer.Serialize(newPricingTable));
        }
    }

    private async Task<IEnumerable<dynamic>> GetPricingFromStore()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var data = await connection.QueryAsync(
            @"SELECT * FROM pricing;");
        return data;
    }

    private static PricingTable CreatePricingTable()
    {
        return new PricingTable(new[]
        {
            new PriceTier(24, 0.5m)
        });
    }

    private async Task CleanUpPricingStore()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("truncate table pricing;");
    }
}