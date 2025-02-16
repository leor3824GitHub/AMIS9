using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Supplier : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Address { get; private set; }
    public string? ContactInfo { get; private set; }
    public string? TIN { get; private set; }
    public string? TaxClassification { get; private set; }
    public string? Email { get; private set; }

    public static Supplier Create(string name, string? address, string? contactInfo, string? tin, string? taxClassification, string? email)
    {
        var supplier = new Supplier();

        supplier.Name = name;
        supplier.Address = address;
        supplier.ContactInfo = contactInfo;
        supplier.TIN = tin;
        supplier.TaxClassification = taxClassification;
        supplier.Email = email;


        supplier.QueueDomainEvent(new SupplierCreated() { Supplier = supplier });

        return supplier;
    }

    public Supplier Update(string? name, string? address, string? contactInfo, string? tin, string? taxClassification, string? email)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (address is not null && Address?.Equals(address, StringComparison.OrdinalIgnoreCase) is not true) Address = address;
        if (contactInfo is not null && ContactInfo?.Equals(contactInfo, StringComparison.OrdinalIgnoreCase) is not true) ContactInfo = contactInfo;
        if (tin is not null && TIN?.Equals(tin, StringComparison.OrdinalIgnoreCase) is not true) TIN = tin;
        if (taxClassification is not null && TaxClassification?.Equals(taxClassification, StringComparison.OrdinalIgnoreCase) is not true) TaxClassification = taxClassification;
        if (email is not null && Email?.Equals(email, StringComparison.OrdinalIgnoreCase) is not true) Email = email;

        this.QueueDomainEvent(new SupplierUpdated() { Supplier = this });
        return this;
    }

    public static Supplier Update(Guid id, string name, string? address, string? contactInfo, string? tin, string? taxClassification, string? email)
    {
        var supplier = new Supplier
        {
            Id = id,
            Name = name,
            Address = address,
            ContactInfo = contactInfo,
            TIN = tin,
            TaxClassification = taxClassification,
            Email = email
        };

        supplier.QueueDomainEvent(new SupplierUpdated() { Supplier = supplier });

        return supplier;
    }
}
