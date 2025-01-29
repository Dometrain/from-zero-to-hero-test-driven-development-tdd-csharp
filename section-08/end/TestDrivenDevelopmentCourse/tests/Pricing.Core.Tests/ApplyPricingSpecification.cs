using FluentAssertions;
using NSubstitute;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;
using Pricing.Core.Tests.TestDoubles;

namespace Pricing.Core.Tests;

public class ApplyPricingSpecification
{
    private static int _maxHourLimit = 24;
    private static int _expectedPrice = 1;

    [Fact]
    public async Task Should_throw_argument_null_exception_if_request_is_null()
    {
        var pricingManager = new PricingManager(new DummyPricingStore());

        var handleRequest = () => pricingManager.HandleAsync(null!, default);

        await handleRequest.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Should_return_true_if_succeeded()
    {
        // arrange
        var pricingManager = new PricingManager(new StubSuccessPricingStore());

        // act
        var result = await pricingManager.HandleAsync(CreateRequest(), default);

        // assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_return_fail_if_fails_to_save()
    {
        var pricingManager = new PricingManager(new StubFailPricingStore());

        var result = await pricingManager.HandleAsync(CreateRequest(), default);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Should_save_only_once()
    {
        var spyPricingService = new SpyPricingService();
        var pricingManager = new PricingManager(spyPricingService);

        _ = await pricingManager.HandleAsync(CreateRequest(), default);

        spyPricingService.NumberOfSaves.Should().Be(_expectedPrice);
    }

    [Fact]
    public async Task Should_save_expected_data()
    {
        var expectedPricingTable = new PricingTable(new[] { new PriceTier(_maxHourLimit, _expectedPrice) });
        var mockPricingStore = new MockPricingStore();
        mockPricingStore.ExpectedToSave(expectedPricingTable);
        var pricingManager = new PricingManager(mockPricingStore);

        _ = await pricingManager.HandleAsync(CreateRequest(), default);

        mockPricingStore.Verify();
    }
    
    [Fact]
    public async Task Should_save_expected_data_nsubstitute()
    {
        var expectedPricingTable = new PricingTable(new[] { new PriceTier(_maxHourLimit, _expectedPrice) });
        var mockPricingStore = Substitute.For<IPricingStore>();
        var pricingManager = new PricingManager(mockPricingStore);

        _ = await pricingManager.HandleAsync(CreateRequest(), default);

        await mockPricingStore.Received().SaveAsync(Arg.Is<PricingTable>(
                table =>
                    // This is only an example on how to use a mocking library. 
                    // Normally, here we would implement equality
                    table.Tiers.Count == expectedPricingTable.Tiers.Count),
            default);
    }

    [Fact]
    public async Task Should_save_equivalent_data_to_storage()
    {
        var pricingStore = new InMemoryPricingStore();
        var pricingManager = new PricingManager(pricingStore);
        var applyPricingRequest = CreateRequest();
        
        _ = await pricingManager.HandleAsync(applyPricingRequest, default);

        pricingStore
            .GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);
    }

    private static ApplyPricingRequest CreateRequest()
    {
        return new ApplyPricingRequest(
            new[] { new PriceTierRequest(_maxHourLimit, _expectedPrice) }
        );
    }
}