using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Update.v1;
public sealed record UpdateProductCommand(
    Guid Id,
    string? Name,
    string? Description = null,
    string? Barcode = null,
    int StockQuantity = 0,
    double AvgPrice = 0,
    double SalePrice = 0,
    string? UnitType = "pc",
    string? BulkUnit = "box",
    int BulkQuantity = 0) : IRequest<UpdateProductResponse>;
