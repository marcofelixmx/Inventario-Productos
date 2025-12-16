using MediatR;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;

namespace Redarbor.Application.Commands.Categories
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category { Name = request.Name };
            await _categoryRepository.AddAsync(category);
            return category.Id;
        }
    }
}
