using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Sale : AuditableEntity, IAggregateRoot
{
    public Guid ProductId { get; private set; }
    public decimal QuantitySold { get; private set; }
    public decimal SalePrice { get; private set; }
    public decimal TotalRevenue { get; private set; }
    public DateTime SaleDate { get; private set; } = default!;

    public static Sale Create(Guid productId, decimal quantitySold, decimal salePrice, decimal totalRevenue, DateTime saleDate)
    {
        var sale = new Sale();

        sale.ProductId = productId;
        sale.QuantitySold = quantitySold;
        sale.SalePrice = salePrice;
        sale.TotalRevenue = totalRevenue;
        sale.SaleDate = saleDate;

        sale.QueueDomainEvent(new SaleCreated() { Sale = sale });

        return sale;
    }

    public Sale Update(Guid? productId, decimal? quantitySold, decimal? salePrice, decimal? totalRevenue, DateTime? saleDate)
    {
        if (productId.HasValue && productId.Value != Guid.Empty && !ProductId.Equals(productId.Value)) ProductId = productId.Value;
        if (quantitySold.HasValue && QuantitySold != quantitySold) QuantitySold = quantitySold.Value;
        if (salePrice.HasValue && SalePrice != salePrice) SalePrice = salePrice.Value;
        if (totalRevenue.HasValue && TotalRevenue != totalRevenue) TotalRevenue = totalRevenue.Value;
        if (saleDate.HasValue && SaleDate != saleDate) SaleDate = saleDate.Value;
        this.QueueDomainEvent(new SaleUpdated() { Sale = this });
        return this;
    }

    public static Sale Update(Guid id, Guid productId, decimal quantitySold, decimal salePrice, decimal totalRevenue, DateTime saleDate)
    {
        var sale = new Sale
        {
            Id = id,
            ProductId = productId,
            QuantitySold = quantitySold,
            SalePrice = salePrice,
            TotalRevenue = totalRevenue,
            SaleDate = saleDate
        };

        sale.QueueDomainEvent(new SaleUpdated() { Sale = sale });

        return sale;
    }
}
