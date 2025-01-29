using Microsoft.AspNetCore.Http.HttpResults;
using Pricing.Core;
using Pricing.Core.Domain.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPricingManager, PricingManager>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPut("/PricingTable", async (IPricingManager pricingManager,
    ApplyPricingRequest request,
    CancellationToken cancellationToken) =>
{
    try
    {
        var result = await pricingManager.HandleAsync(request, cancellationToken);
        return result ? Results.Ok() : Results.BadRequest();
    }
    catch (InvalidPricingTierException)
    {
        return Results.Problem();
    }
});

app.Run();