using System.Collections.Immutable;

namespace Pricing.Core.Domain;

public class PricingTable
{
    private readonly decimal? _maxDailyPrice;

    public PricingTable(IEnumerable<PriceTier> tiers, decimal? maxDailyPrice = null)
    {
        _maxDailyPrice = maxDailyPrice;
        Tiers = tiers?.OrderBy(tier => tier.HourLimit).ToImmutableArray() ?? throw new ArgumentNullException();

        if (!Tiers.Any())
            throw new ArgumentException("Missing Pricing Tiers", nameof(Tiers));

        if (Tiers.Last().HourLimit < 24) throw new ArgumentException();

        if (_maxDailyPrice.HasValue && _maxDailyPrice.Value > CalculateMaxDailyPriceFromTiers())
            throw new ArgumentException();
    }

    public IReadOnlyCollection<PriceTier> Tiers { get; }

    public decimal GetMaxDailyPrice()
    {
        if (_maxDailyPrice.HasValue)
            return _maxDailyPrice.Value;

        return CalculateMaxDailyPriceFromTiers();
    }

    private decimal CalculateMaxDailyPriceFromTiers()
    {
        decimal total = 0;
        var hoursIncluded = 0;
        foreach (var tier in Tiers)
        {
            total += tier.Price * (tier.HourLimit - hoursIncluded);
            hoursIncluded = tier.HourLimit;
        }

        return total;
    }
}