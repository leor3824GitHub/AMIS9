using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Domain;
using FSH.Starter.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Delete.v1;
public sealed class DeleteProductHandler(
    ILogger<DeleteProductHandler> logger,
    [FromKeyedServices("catalog:products")] IRepository<Product> repository)
    : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var count = await repository.CountAsync(cancellationToken);
        logger.LogInformation("Total items before delete : {ItemCount} ", count);
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = product ?? throw new ProductNotFoundException(request.Id);
        await repository.DeleteAsync(product, cancellationToken);
        //await repository.SaveChangesAsync(cancellationToken); // Ensure changes are saved to the database
        logger.LogInformation("Product with id : {ProductId} deleted", product.Id);

        //ArgumentNullException.ThrowIfNull(request);

        //var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        //_ = product ?? throw new ProductNotFoundException(request.Id);

        //// 🔹 Soft delete: Mark product as deleted
        //product.Deleted = DateTimeOffset.UtcNow;

        //await repository.UpdateAsync(product, cancellationToken); // ✅ Use UpdateAsync
        //await repository.SaveChangesAsync(cancellationToken); // ✅ Persist changes

        //logger.LogInformation("Product with id: {ProductId} was soft deleted by user: {UserId}",
        //    product.Id, product.DeletedBy);
    }
}
