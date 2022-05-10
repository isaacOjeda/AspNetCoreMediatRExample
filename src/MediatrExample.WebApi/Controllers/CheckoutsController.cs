using MediatR;
using MediatrExample.ApplicationCore.Features.Checkouts.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediatrExample.WebApi.Controllers;


[Authorize]
[ApiController]
[Route("api/checkouts")]
public class CheckoutsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CheckoutsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crea una nueva orden de productos
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> NewCheckout([FromBody] NewCheckoutCommand command)
    {
        await _mediator.Send(command);

        return Accepted();
    }
}
