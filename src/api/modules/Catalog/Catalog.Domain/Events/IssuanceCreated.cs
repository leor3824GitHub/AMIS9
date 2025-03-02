using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain.Events;
public sealed record IssuanceCreated : DomainEvent
{
    public Issuance? Issuance { get; set; }
}
