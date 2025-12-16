using MediatR;

namespace Redarbor.Application.Commands.InventoryMovements
{
    public record CreateInventoryMovementCommand(int ProductId, int Quantity) : IRequest<int>;
}
