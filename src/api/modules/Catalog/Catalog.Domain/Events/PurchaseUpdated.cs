using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
// ...existing code...
public sealed record PurchaseUpdated : DomainEvent
{
    public Purchase? Purchase { get; set; }
}
