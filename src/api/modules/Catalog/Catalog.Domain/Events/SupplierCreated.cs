using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
public sealed record SupplierCreated : DomainEvent
{
    public Supplier? Supplier { get; set; }
}
