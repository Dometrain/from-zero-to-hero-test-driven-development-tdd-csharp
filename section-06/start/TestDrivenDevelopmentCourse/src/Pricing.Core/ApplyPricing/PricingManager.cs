using Pricing.Core.Extensions;

namespace Pricing.Core.ApplyPricing;

public class PricingManager : IPricingManager
{
    private readonly IPricingStore _pricingStore;

    public PricingManager(IPricingStore pricingStore)
    {
        _pricingStore = pricingStore;
    }
    
    public async Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pricingTable = request.ToPricingTable();
        
        return await _pricingStore.SaveAsync(pricingTable, cancellationToken);
    }
}