using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Issuance : AuditableEntity, IAggregateRoot
{
    public Guid ProductId { get; private set; }
    public decimal Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime Date { get; private set; } = default!;
    public string Unit { get; private set; } = default!;

    public static Issuance Create(Guid productId, decimal qty, decimal unitPrice, decimal totalAmount, DateTime date, string unit)
    {
        var issuance = new Issuance
        {
            ProductId = productId,
            Qty = qty,
            UnitPrice = unitPrice,
            TotalAmount = totalAmount,
            Date = date,
            Unit = unit
        };

        issuance.QueueDomainEvent(new IssuanceCreated() { Issuance = issuance });

        return issuance;
    }

    public Issuance Update(Guid? productId, decimal? qty, decimal? unitPrice, decimal? totalAmount, DateTime? date, string? unit)
    {
        if (productId.HasValue && productId.Value != Guid.Empty && !ProductId.Equals(productId.Value)) ProductId = productId.Value;
        if (qty.HasValue && Qty != qty) Qty = qty.Value;
        if (unitPrice.HasValue && UnitPrice != unitPrice) UnitPrice = unitPrice.Value;
        if (totalAmount.HasValue && TotalAmount != totalAmount) TotalAmount = totalAmount.Value;
        if (date.HasValue && Date != date) Date = date.Value;
        if (!string.IsNullOrEmpty(unit) && !Unit.Equals(unit, StringComparison.Ordinal)) Unit = unit;

        this.QueueDomainEvent(new IssuanceUpdated() { Issuance = this });
        return this;
    }

    public static Issuance Update(Guid id, Guid productId, decimal qty, decimal unitPrice, decimal totalAmount, DateTime date, string unit)
    {
        var issuance = new Issuance
        {
            Id = id,
            ProductId = productId,
            Qty = qty,
            UnitPrice = unitPrice,
            TotalAmount = totalAmount,
            Date = date,
            Unit = unit
        };

        issuance.QueueDomainEvent(new IssuanceUpdated() { Issuance = issuance });

        return issuance;
    }
}
