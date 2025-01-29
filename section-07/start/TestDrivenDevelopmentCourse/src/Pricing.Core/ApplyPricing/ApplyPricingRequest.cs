namespace Pricing.Core.ApplyPricing;

public record ApplyPricingRequest(IReadOnlyCollection<PriceTierRequest> Tiers);

public record PriceTierRequest(int HourLimit, decimal Price);