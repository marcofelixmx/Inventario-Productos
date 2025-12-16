using MediatR;

namespace Redarbor.Application.Commands.Categories
{
    public record DeleteCategoryCommand(int Id) : IRequest<bool>;
}
