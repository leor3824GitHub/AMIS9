using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Purchase : AuditableEntity, IAggregateRoot
{
    public string? ReferenceNo { get; private set; }
    public Guid ProductId { get; private set; }
    public DateTime PurchaseDate { get; private set; } = default!;
    public Guid SupplierId { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalAmount { get; private set; }

    public static Purchase Create(Guid productId, Guid supplierId, decimal quantity, decimal unitPrice, decimal totalAmount, DateTime purchaseDate, string referenceNo)
    {
        var purchase = new Purchase
        {
            ProductId = productId,
            SupplierId = supplierId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            TotalAmount = totalAmount,
            PurchaseDate = purchaseDate,
            ReferenceNo = referenceNo
        };

        purchase.QueueDomainEvent(new PurchaseCreated { Purchase = purchase });

        return purchase;
    }

    public Purchase Update(Guid? productId, Guid? supplierId, decimal? quantity, decimal? unitPrice, decimal? totalAmount, DateTime? purchaseDate, string? referenceNo)
    {
        if (productId.HasValue && productId.Value != Guid.Empty && !ProductId.Equals(productId.Value)) ProductId = productId.Value;
        if (supplierId.HasValue && supplierId.Value != Guid.Empty && !SupplierId.Equals(supplierId.Value)) SupplierId = supplierId.Value;
        if (quantity.HasValue && Quantity != quantity) Quantity = quantity.Value;
        if (unitPrice.HasValue && UnitPrice != unitPrice) UnitPrice = unitPrice.Value;
        if (totalAmount.HasValue && TotalAmount != totalAmount) TotalAmount = totalAmount.Value;
        if (purchaseDate.HasValue && PurchaseDate != purchaseDate) PurchaseDate = purchaseDate.Value;
        if (!string.IsNullOrEmpty(referenceNo) && !ReferenceNo!.Equals(referenceNo, StringComparison.Ordinal)) ReferenceNo = referenceNo;

        this.QueueDomainEvent(new PurchaseUpdated { Purchase = this });
        return this;
    }

    public static Purchase Update(Guid id, Guid productId, Guid supplierId, decimal quantity, decimal unitPrice, decimal totalAmount, DateTime purchaseDate, string referenceNo)
    {
        var purchase = new Purchase
        {
            Id = id,
            ProductId = productId,
            SupplierId = supplierId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            TotalAmount = totalAmount,
            PurchaseDate = purchaseDate,
            ReferenceNo = referenceNo
        };

        purchase.QueueDomainEvent(new PurchaseUpdated { Purchase = purchase });

        return purchase;
    }
}
