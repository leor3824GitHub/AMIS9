using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class PurchaseConfiguration: IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder) 
        { 
            builder.IsMultiTenant();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ReferenceNo).HasMaxLength(100);
            builder.Property(x => x.PurchaseDate).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.SupplierId).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.UnitPrice).HasDefaultValue(0);
        }
    }
}
