using System.Text.Json;
using Finbuckle.MultiTenant;
using FSH.Framework.Core.Serialization;
using FSH.Starter.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Ignore(e => e.DomainEvents);
        builder.Property(e => e.Pictures)
            .HasConversion(
                v => JsonSerializer.Serialize(v, DefaultJsonSerializerOptions.Options),
                v => JsonSerializer.Deserialize<List<ProductImage>>(v, DefaultJsonSerializerOptions.Options),
                new ValueComparer<List<ProductImage>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }
}
