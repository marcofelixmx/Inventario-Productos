using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Queries.Products;

namespace Redarbor.Tests.Handlers.Products
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new GetAllProductsQueryHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.00m, Stock = 100, CategoryId = 1, Status = true },
                new Product { Id = 2, Name = "Product 2", Price = 20.00m, Stock = 200, CategoryId = 1, Status = true }
            };
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            var query = new GetAllProductsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(products);
            _mockProductRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Product>());

            var query = new GetAllProductsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _mockProductRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
