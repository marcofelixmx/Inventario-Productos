using MediatR;
using Redarbor.Core.Domain;

namespace Redarbor.Application.Queries.Products
{
    public record GetProductByIdQuery(int Id) : IRequest<Product>;
}
