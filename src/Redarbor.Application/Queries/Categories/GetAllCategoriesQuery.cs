using MediatR;
using Redarbor.Core.Domain;

namespace Redarbor.Application.Queries.Categories
{
    public record GetAllCategoriesQuery : IRequest<IEnumerable<Category>>;
}
