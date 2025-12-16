using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Queries.Categories;

namespace Redarbor.Tests.Handlers.Categories
{
    public class GetAllCategoriesQueryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly GetAllCategoriesQueryHandler _handler;

        public GetAllCategoriesQueryHandlerTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _handler = new GetAllCategoriesQueryHandler(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            var query = new GetAllCategoriesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(categories);
            _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Category>());

            var query = new GetAllCategoriesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
