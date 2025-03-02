using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
// ...existing code...
public sealed record PurchaseCreated : DomainEvent
{
    public Purchase? Purchase { get; set; }
}
