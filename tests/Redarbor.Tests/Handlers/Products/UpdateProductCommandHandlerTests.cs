using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Commands.Products;

namespace Redarbor.Tests.Handlers.Products
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new UpdateProductCommandHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "Old Name", Price = 10.00m, Stock = 100, CategoryId = 1, Status = true };
            var command = new UpdateProductCommand(1, "New Name", "New Description", 15.00m, 120, 2, false);

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Name == command.Name && p.Status == command.Status)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new UpdateProductCommand(99, "Non Existent", "Description", 15.00m, 120, 2, false);

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}
