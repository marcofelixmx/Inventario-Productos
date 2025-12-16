using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Commands.Products;

namespace Redarbor.Tests.Handlers.Products
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "Product to Delete", Price = 10.00m, Stock = 100, CategoryId = 1, Status = true };
            var command = new DeleteProductCommand(1);

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockProductRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteProductCommand(99);

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
            _mockProductRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
