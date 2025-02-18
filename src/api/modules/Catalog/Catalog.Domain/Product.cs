using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? BaseUnit { get; private set; }
    public decimal ConversionFactor { get; private set; } = 1;
    public string? BulkUnit { get; private set; }
    public Guid? BrandId { get; private set; }
    public virtual Brand Brand { get; private set; } = default!;

    public static Product Create(string name, string? description, Guid? brandId, string? baseUnit, decimal conversionFactor, string? bulkUnit)
    {
        var product = new Product
        {
            Name = name,
            Description = description,
            BrandId = brandId,
            BaseUnit = baseUnit,
            ConversionFactor = conversionFactor,
            BulkUnit = bulkUnit
        };

        product.QueueDomainEvent(new ProductCreated() { Product = product });

        return product;
    }

    public Product Update(string? name, string? description, Guid? brandId, string? baseUnit, decimal? conversionFactor, string? bulkUnit)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;
        if (brandId.HasValue && brandId.Value != Guid.Empty && !BrandId.Equals(brandId.Value)) BrandId = brandId.Value;
        if (baseUnit is not null && BaseUnit?.Equals(baseUnit, StringComparison.OrdinalIgnoreCase) is not true) BaseUnit = baseUnit;
        if (conversionFactor.HasValue && ConversionFactor != conversionFactor) ConversionFactor = conversionFactor.Value;
        if (bulkUnit is not null && BulkUnit?.Equals(bulkUnit, StringComparison.OrdinalIgnoreCase) is not true) BulkUnit = bulkUnit;

        this.QueueDomainEvent(new ProductUpdated() { Product = this });
        return this;
    }

    public static Product Update(Guid id, string name, string? description, Guid? brandId, string? baseUnit, decimal conversionFactor, string? bulkUnit)
    {
        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            BrandId = brandId,
            BaseUnit = baseUnit,
            ConversionFactor = conversionFactor,
            BulkUnit = bulkUnit
        };

        product.QueueDomainEvent(new ProductUpdated() { Product = product });

        return product;
    }
}
