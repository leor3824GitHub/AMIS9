using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
public sealed record SupplierUpdated : DomainEvent
{
    public Supplier? Supplier { get; set; }
}
