using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Queries.Categories;

namespace Redarbor.Tests.Handlers.Categories
{
    public class GetCategoryByIdQueryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly GetCategoryByIdQueryHandler _handler;

        public GetCategoryByIdQueryHandlerTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _handler = new GetCategoryByIdQueryHandler(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);

            var query = new GetCategoryByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(category);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            var query = new GetCategoryByIdQuery(99);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
        }
    }
}
