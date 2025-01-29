namespace Pricing.Core.ApplyPricing;

public interface IPricingManager
{
    Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken);
}