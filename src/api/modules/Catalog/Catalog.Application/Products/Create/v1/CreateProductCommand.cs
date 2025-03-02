using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Create.v1;
public sealed record CreateProductCommand(
    [property: DefaultValue("Sample Product")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null,
    [property: DefaultValue("1234567890123")] string? Barcode = null,
    [property: DefaultValue(0)] int StockQuantity = 0,
    [property: DefaultValue(0)] double AvgPrice = 0,
    [property: DefaultValue(0)] double SalePrice = 0,
    [property: DefaultValue("pcs")] string? UnitType = null,
    [property: DefaultValue(0)] int BulkQuantity = 0,
    [property: DefaultValue("box")] string? BulkUnit = null) : IRequest<CreateProductResponse>;
