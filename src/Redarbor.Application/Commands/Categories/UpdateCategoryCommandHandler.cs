using MediatR;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;

namespace Redarbor.Application.Commands.Categories
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                return false;
            }

            category.Name = request.Name;
            await _categoryRepository.UpdateAsync(category);
            return true;
        }
    }
}
