using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
public sealed record ProductInventoryCreated : DomainEvent
{
    public ProductInventory? ProductInventory { get; set; }
}
