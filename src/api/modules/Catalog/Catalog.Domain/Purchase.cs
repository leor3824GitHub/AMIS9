using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Purchase : AuditableEntity, IAggregateRoot
{
    public Guid Productid { get; private set; }
    public Guid Supplierid { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Unitprice { get; private set; }
    public decimal Totalprice { get; private set; }
    public DateTime Purchasedate { get; private set; } = default!;

    public static Purchase Create(Guid productid, Guid supplierid, decimal quantity, decimal unitprice, decimal totalprice, DateTime purchasedate)
    {
        var purchase = new Purchase();

        purchase.Productid = productid;
        purchase.Supplierid = supplierid;
        purchase.Quantity = quantity;
        purchase.Unitprice = unitprice;
        purchase.Totalprice = totalprice;
        purchase.Purchasedate = purchasedate;

        purchase.QueueDomainEvent(new PurchaseCreated() { Purchase = purchase });

        return purchase;
    }

    public Purchase Update(Guid? productid, Guid? supplierid, decimal? quantity, decimal? unitprice, decimal? totalprice, DateTime? purchasedate)
    {
        if (productid.HasValue && productid.Value != Guid.Empty && !Productid.Equals(productid.Value)) Productid = productid.Value;
        if (supplierid.HasValue && supplierid.Value != Guid.Empty && !Supplierid.Equals(supplierid.Value)) Supplierid = supplierid.Value;
        if (quantity.HasValue && Quantity != quantity) Quantity = quantity.Value;
        if (unitprice.HasValue && Unitprice != unitprice) Unitprice = unitprice.Value;
        if (totalprice.HasValue && Totalprice != totalprice) Totalprice = totalprice.Value;
        if (purchasedate.HasValue && Purchasedate != purchasedate) Purchasedate = purchasedate.Value;
        this.QueueDomainEvent(new PurchaseUpdated() { Purchase = this });
        return this;
    }

    public static Purchase Update(Guid id, Guid productid, Guid supplierid, decimal quantity, decimal unitprice, decimal totalprice, DateTime purchasedate)
    {
        var purchase = new Purchase
        {
            Id = id,
            Productid = productid,
            Supplierid = supplierid,
            Quantity = quantity,
            Unitprice = unitprice,
            Totalprice = totalprice,
            Purchasedate = purchasedate
        };

        purchase.QueueDomainEvent(new PurchaseUpdated() { Purchase = purchase });

        return purchase;
    }
}
