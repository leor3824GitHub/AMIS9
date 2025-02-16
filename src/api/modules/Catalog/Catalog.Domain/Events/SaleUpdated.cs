using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
public sealed record SaleUpdated : DomainEvent
{
    public Sale? Sale { get; set; }
}
