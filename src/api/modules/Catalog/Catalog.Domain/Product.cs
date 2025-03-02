using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? Barcode { get; private set; }
    public int StockQuantity { get; private set; }
    public double AvgPrice { get; private set; }
    public double SalePrice { get; private set; }
    public string? UnitType { get; private set; }
    public string? BulkUnit { get; private set; }
    public int BulkQuantity { get; private set; }
    public List<ProductImage>? Pictures { get; private set; } = [];

    public static Product Create(string name, string? description, string? barcode, int stockQuantity, double avgPrice, double salePrice, string? unitType, string? bulkUnit, int bulkQuantity, List<ProductImage>? pictures)
    {
        var product = new Product
        {
            Name = name,
            Description = description,
            Barcode = barcode,
            StockQuantity = stockQuantity,
            AvgPrice = avgPrice,
            SalePrice = salePrice,
            UnitType = unitType,
            BulkUnit = bulkUnit,
            BulkQuantity = bulkQuantity,
            Pictures = pictures ?? new List<ProductImage>()
        };

        product.QueueDomainEvent(new ProductCreated() { Product = product });

        return product;
    }

    public Product Update(string? name, string? description, string? barcode, int? stockQuantity, double? avgPrice, double? salePrice, string? unitType, string? bulkUnit, int? bulkQuantity, List<ProductImage>? pictures)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;
        if (barcode is not null && Barcode?.Equals(barcode, StringComparison.OrdinalIgnoreCase) is not true) Barcode = barcode;
        if (stockQuantity.HasValue && StockQuantity != stockQuantity) StockQuantity = stockQuantity.Value;
        if (avgPrice.HasValue && AvgPrice != avgPrice) AvgPrice = avgPrice.Value;
        if (salePrice.HasValue && SalePrice != salePrice) SalePrice = salePrice.Value;
        if (unitType is not null && UnitType?.Equals(unitType, StringComparison.OrdinalIgnoreCase) is not true) UnitType = unitType;
        if (bulkUnit is not null && BulkUnit?.Equals(bulkUnit, StringComparison.OrdinalIgnoreCase) is not true) BulkUnit = bulkUnit;
        if (bulkQuantity.HasValue && BulkQuantity != bulkQuantity) BulkQuantity = bulkQuantity.Value;
        if (pictures is not null && !Pictures.SequenceEqual(pictures)) Pictures = pictures;

        this.QueueDomainEvent(new ProductUpdated() { Product = this });
        return this;
    }

    public static Product Update(Guid id, string name, string? description, string? barcode, int stockQuantity, double avgPrice, double salePrice, string? unitType, string? bulkUnit, int bulkQuantity, List<ProductImage>? pictures)
    {
        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Barcode = barcode,
            StockQuantity = stockQuantity,
            AvgPrice = avgPrice,
            SalePrice = salePrice,
            UnitType = unitType,
            BulkUnit = bulkUnit,
            BulkQuantity = bulkQuantity,
            Pictures = pictures ?? new List<ProductImage>()
        };

        product.QueueDomainEvent(new ProductUpdated() { Product = product });

        return product;
    }
}
public class ProductImage
{
    public required string Name { get; set; }
    public decimal Size { get; set; }
    public required string Url { get; set; }
}
