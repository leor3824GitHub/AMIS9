using FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;
public sealed record ProductResponse(Guid? Id, string name, string? description, string? barcode, int stockQuantity, double avgPrice, double salePrice, string? unitType, string? bulkUnit, int bulkQuantity);
