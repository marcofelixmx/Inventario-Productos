using MediatR;

namespace Redarbor.Application.Commands.Categories
{
    public record CreateCategoryCommand(string Name) : IRequest<int>;
}
