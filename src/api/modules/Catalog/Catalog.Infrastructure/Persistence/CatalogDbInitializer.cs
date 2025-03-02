using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Persistence;
internal sealed class CatalogDbInitializer(
    ILogger<CatalogDbInitializer> logger,
    CatalogDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for catalog module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Seed default catalog data of 15 random products
        var random = new Random();
        var products = new List<Product>();

        for (int i = 0; i < 15; i++)
        {
            string name = $"Product {i + 1}";
            string description = $"Description for product {i + 1}";
            string barcode = random.Next(100000000, 999999999).ToString();
            int stockQuantity = random.Next(1, 100);
            double avgPrice = random.NextDouble() * 100;
            double salePrice = avgPrice * (random.NextDouble() * 0.5 + 0.75); // Sale price between 75% and 125% of avg price
            string unitType = "Each";
            string bulkUnit = "Box";
            int bulkQuantity = random.Next(1, 50);
            List<ProductImage>? pictures = null;

            var product = Product.Create(name, description, barcode, stockQuantity, avgPrice, salePrice, unitType, bulkUnit, bulkQuantity, pictures);
            products.Add(product);
        }

        if (!await context.Products.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            await context.Products.AddRangeAsync(products, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default catalog data", context.TenantInfo!.Identifier);
        }
    }

}
