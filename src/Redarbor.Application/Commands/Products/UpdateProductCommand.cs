using MediatR;

namespace Redarbor.Application.Commands.Products
{
    public record UpdateProductCommand(int Id, string Name, string Description, decimal Price, int Stock, int CategoryId, bool Status) : IRequest<bool>;
}
