using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class ProductInventory : AuditableEntity, IAggregateRoot
{
    public Guid ProductId { get; private set; }
    public decimal Stockquantity { get; private set; }
    public decimal Averageprice { get; private set; }
    public virtual Product Product { get; private set; } = default!;

    public static ProductInventory Create(Guid productId, decimal stockquantity, decimal averageprice)
    {
        var productinventory = new ProductInventory();

        productinventory.ProductId = productId;
        productinventory.Stockquantity = stockquantity;
        productinventory.Stockquantity = stockquantity;

        productinventory.QueueDomainEvent(new ProductInventoryUpdated() { ProductInventory = productinventory });

        return productinventory;
    }

    public ProductInventory Update(Guid? productId, decimal? stockquantity, decimal? averageprice)
    {
        if (productId.HasValue && productId.Value != Guid.Empty && !ProductId.Equals(productId.Value)) ProductId = productId.Value;
        if (stockquantity.HasValue && Stockquantity != stockquantity) Stockquantity = stockquantity.Value;
        if (stockquantity.HasValue && Stockquantity != averageprice) Stockquantity = stockquantity.Value;
        this.QueueDomainEvent(new ProductInventoryUpdated() { ProductInventory = this });
        return this;
    }

    public static ProductInventory Update(Guid id, Guid productId, decimal stockquantity, decimal averageprice)
    {
        var productinventory = new ProductInventory
        {
            Id = id,
            ProductId = productId,
            Stockquantity = stockquantity,
            Averageprice = averageprice
        };

        productinventory.QueueDomainEvent(new ProductInventoryUpdated() { ProductInventory = productinventory });

        return productinventory;
    }
}
