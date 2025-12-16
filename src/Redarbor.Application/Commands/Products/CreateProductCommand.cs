using MediatR;

namespace Redarbor.Application.Commands.Products
{
    public record CreateProductCommand(string Name, string Description, decimal Price, int Stock, int CategoryId, bool Status) : IRequest<int>;
}
