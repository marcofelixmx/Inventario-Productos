using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Commands.Categories;

namespace Redarbor.Tests.Handlers.Categories
{
    public class UpdateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly UpdateCategoryCommandHandler _handler;

        public UpdateCategoryCommandHandlerTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _handler = new UpdateCategoryCommandHandler(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCategory_WhenCategoryExists()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "Old Category" };
            var command = new UpdateCategoryCommand(1, "New Category");

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.Is<Category>(c => c.Name == command.Name)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new UpdateCategoryCommand(99, "Non Existent");

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }
    }
}
