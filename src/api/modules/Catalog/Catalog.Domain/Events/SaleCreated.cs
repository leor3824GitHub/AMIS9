using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
public sealed record SaleCreated : DomainEvent
{
    public Sale? Sale { get; set; }
}
