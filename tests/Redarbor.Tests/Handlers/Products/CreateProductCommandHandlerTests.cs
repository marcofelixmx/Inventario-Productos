using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Commands.Products;

namespace Redarbor.Tests.Handlers.Products
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new CreateProductCommandHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProductAndReturnId()
        {
            // Arrange
            var command = new CreateProductCommand("New Product", "Description", 15.00m, 50, 1, true);
            var product = new Product { Id = 1, Name = command.Name, Description = command.Description, Price = command.Price, Stock = command.Stock, CategoryId = command.CategoryId, Status = command.Status };

            _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => p.Id = product.Id) // Simulate DB assigning an ID
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(product.Id);
            _mockProductRepository.Verify(repo => repo.AddAsync(It.Is<Product>(p => p.Name == command.Name)), Times.Once);
        }
    }
}
