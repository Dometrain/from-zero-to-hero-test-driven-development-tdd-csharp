using FluentAssertions;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles;

public class MockPricingStore : IPricingStore
{
    private PricingTable _expectedPricingTable;
    private PricingTable _savedPricingTable;

    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        _savedPricingTable = pricingTable;
        return Task.FromResult(true);
    }

    public void Verify()
    {
        _savedPricingTable.Should().BeEquivalentTo(_expectedPricingTable);
    }

    public void ExpectedToSave(PricingTable expectedPricingTable)
    {
        _expectedPricingTable = expectedPricingTable;
    }
}