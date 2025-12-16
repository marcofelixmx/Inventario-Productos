using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Commands.Categories;

namespace Redarbor.Tests.Handlers.Categories
{
    public class DeleteCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly DeleteCategoryCommandHandler _handler;

        public DeleteCategoryCommandHandlerTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _handler = new DeleteCategoryCommandHandler(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteCategory_WhenCategoryExists()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "Category to Delete" };
            var command = new DeleteCategoryCommand(1);

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new DeleteCategoryCommand(99);

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
