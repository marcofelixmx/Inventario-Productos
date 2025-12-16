using Redarbor.Core.Domain;

namespace Redarbor.Core.Interfaces
{
    public interface IInventoryMovementRepository
    {
        Task AddAsync(InventoryMovement inventoryMovement);
    }
}
