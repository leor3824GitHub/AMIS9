using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Update.v1;
public sealed record UpdateProductCommand(
    Guid Id,
    string? Name,
    string? Description = null,
    Guid? BrandId = null,
    string? BaseUnit = null,
    decimal ConversionFactor = 1,
    string? BulkUnit = null) : IRequest<UpdateProductResponse>;
