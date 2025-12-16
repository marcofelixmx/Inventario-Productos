using Moq;
using Xunit;
using FluentAssertions;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Application.Commands.InventoryMovements;

namespace Redarbor.Tests.Handlers.InventoryMovements
{
    public class CreateInventoryMovementCommandHandlerTests
    {
        private readonly Mock<IInventoryMovementRepository> _mockInventoryMovementRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CreateInventoryMovementCommandHandler _handler;

        public CreateInventoryMovementCommandHandlerTests()
        {
            _mockInventoryMovementRepository = new Mock<IInventoryMovementRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new CreateInventoryMovementCommandHandler(_mockInventoryMovementRepository.Object, _mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateMovementAndUpdateProductStock_WhenProductExists()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "Test Product", Stock = 100 };
            var command = new CreateInventoryMovementCommand(1, 50); // Add 50 to stock

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _mockInventoryMovementRepository.Setup(repo => repo.AddAsync(It.IsAny<InventoryMovement>()))
                .Callback<InventoryMovement>(im => im.Id = 1) // Simulate DB assigning an ID
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1); // Expected ID
            existingProduct.Stock.Should().Be(150); // Stock updated
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Stock == 150)), Times.Once);
            _mockInventoryMovementRepository.Verify(repo => repo.AddAsync(It.Is<InventoryMovement>(im => im.ProductId == 1 && im.Quantity == 50)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new CreateInventoryMovementCommand(99, 50); // Product 99 does not exist

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Product not found.");
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _mockInventoryMovementRepository.Verify(repo => repo.AddAsync(It.IsAny<InventoryMovement>()), Times.Never);
        }
    }
}
