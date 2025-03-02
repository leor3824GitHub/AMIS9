using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;

public sealed record IssuanceUpdated : DomainEvent
{
    public Issuance? Issuance { get; set; }
}
