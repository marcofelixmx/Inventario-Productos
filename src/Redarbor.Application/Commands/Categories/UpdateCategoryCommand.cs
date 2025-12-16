using MediatR;

namespace Redarbor.Application.Commands.Categories
{
    public record UpdateCategoryCommand(int Id, string Name) : IRequest<bool>;
}
