using application.Menus.Queries;
using application.Menus.Queries.Dto;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webapi.Controllers.Menus.Requests;

namespace webapi.Controllers.Menus;

public class MenuController : ApiController
{
    public MenuController(IMediator mediator) : base(mediator)
    {
    }

    //TODO: change to single element
    //TODO: make some flag to mark as active
    [HttpPost]
    [SwaggerOperation(Summary = "Create menu for restaurant.")]
    [SwaggerResponse(200, "", typeof(Result<MenuId>))]
    [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
    public async Task<IActionResult> CreateMenu(CreateMenusRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.CastToCommand(), cancellationToken);
        return MapApplicationResultToHttpResponse(result);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get restaurant menus.")]
    [SwaggerResponse(200, "", typeof(MenuRestaurantDto))]
    [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
    public async Task<IActionResult> GetMenus(int restaurantId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRestaurantMenusQuery(restaurantId), cancellationToken);
        if (result is null)
            return BadRequest();
        
        return Ok(result);
    }
}