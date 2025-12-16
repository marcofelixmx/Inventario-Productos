using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Commands.Categories;

namespace Redarbor.Tests.Handlers.Categories
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CreateCategoryCommandHandler _handler;

        public CreateCategoryCommandHandlerTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _handler = new CreateCategoryCommandHandler(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateCategoryAndReturnId()
        {
            // Arrange
            var command = new CreateCategoryCommand("New Category");
            var category = new Category { Id = 1, Name = command.Name };

            _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .Callback<Category>(c => c.Id = category.Id) // Simulate DB assigning an ID
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(category.Id);
            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == command.Name)), Times.Once);
        }
    }
}
