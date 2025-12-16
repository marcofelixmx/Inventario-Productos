using MediatR;
using Microsoft.AspNetCore.Mvc;
using Redarbor.Application.Commands.InventoryMovements;
using Microsoft.AspNetCore.Authorization;

namespace Redarbor.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryMovementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryMovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateInventoryMovement([FromBody] CreateInventoryMovementCommand command)
        {
            var movementId = await _mediator.Send(command);
            return Ok(movementId); // Or CreatedAtAction if there was a GetById for movements
        }
    }
}
