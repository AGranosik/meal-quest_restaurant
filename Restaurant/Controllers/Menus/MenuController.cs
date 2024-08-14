using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webapi.Controllers.Menus.Requests;

namespace webapi.Controllers.Menus
{
    public class MenuController(IMediator mediator) : ApiController(mediator)
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Create menu for restaurant.")]
        [SwaggerResponse(200, "", typeof(Result<MenuId>))]
        [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
        public async Task<IActionResult> CreateMenuAsync(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request.CastToCommand(), cancellationToken);
            return MapApplicationResultToHttpResponse(result);
        }
    }
}
