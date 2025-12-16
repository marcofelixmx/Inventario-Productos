using MediatR;
using Redarbor.Core.Domain;

namespace Redarbor.Application.Queries.Products
{
    public record GetAllProductsQuery : IRequest<IEnumerable<Product>>;
}
