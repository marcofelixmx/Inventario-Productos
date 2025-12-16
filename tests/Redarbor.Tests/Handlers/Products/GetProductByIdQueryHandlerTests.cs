using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Queries.Products;

namespace Redarbor.Tests.Handlers.Products
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new GetProductByIdQueryHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 10.00m, Stock = 100, CategoryId = 1, Status = true };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            var query = new GetProductByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var query = new GetProductByIdQuery(99);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
        }
    }
}
