using FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;
public sealed record ProductResponse(Guid? Id, string Name, string? Description, BrandResponse? Brand, string? BaseUnit, decimal ConversionFactor, string? BulkUnit);
