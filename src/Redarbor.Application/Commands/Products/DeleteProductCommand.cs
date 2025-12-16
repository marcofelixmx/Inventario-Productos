using MediatR;

namespace Redarbor.Application.Commands.Products
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;
}
