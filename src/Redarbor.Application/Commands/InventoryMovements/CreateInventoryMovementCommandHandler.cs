using MediatR;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;

namespace Redarbor.Application.Commands.InventoryMovements
{
    public class CreateInventoryMovementCommandHandler : IRequestHandler<CreateInventoryMovementCommand, int>
    {
        private readonly IInventoryMovementRepository _inventoryMovementRepository;
        private readonly IProductRepository _productRepository; // To update product stock

        public CreateInventoryMovementCommandHandler(IInventoryMovementRepository inventoryMovementRepository, IProductRepository productRepository)
        {
            _inventoryMovementRepository = inventoryMovementRepository;
            _productRepository = productRepository;
        }

        public async Task<int> Handle(CreateInventoryMovementCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                // Handle case where product does not exist
                throw new Exception("Product not found."); // Or return a specific error type
            }

            // Update product stock
            product.Stock += request.Quantity;
            await _productRepository.UpdateAsync(product);

            var inventoryMovement = new InventoryMovement
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                MovementDate = DateTime.UtcNow
            };

            await _inventoryMovementRepository.AddAsync(inventoryMovement);

            return inventoryMovement.Id;
        }
    }
}
