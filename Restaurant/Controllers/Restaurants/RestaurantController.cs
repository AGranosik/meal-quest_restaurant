using application.Restaurants.Queries.OwnerRestaurantQueries;
using application.Restaurants.Queries.OwnerRestaurantQueries.Dto;
using application.Restaurants.Queries.RestaurantDetailsQueries;
using application.Restaurants.Queries.RestaurantDetailsQueries.Dto;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webapi.Controllers.Restaurants.Requests;

namespace webapi.Controllers.Restaurants;

public class RestaurantController : ApiController
{
    public RestaurantController(IMediator mediator, ILogger<RestaurantController> logger) : base(mediator)
    {
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create restaurant.")]
    [SwaggerResponse(200, "", typeof(Result<RestaurantId>))]
    [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
    public async Task<IActionResult> CreateRestaurantAsync([FromForm] CreateRestaurantRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(await request.CastToCommand(), cancellationToken);
        return MapApplicationResultToHttpResponse(result);
    }

    [HttpGet("owner")]
    [SwaggerOperation(Summary = "Get owner's restaurants.")]
    [SwaggerResponse(200, "", typeof(List<OwnerRestaurantDto>))]
    [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
    public async Task<IActionResult> GetOwnersRestaurantAsync([FromQuery] int ownerId, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new OwnerRestaurantQuery
        {
            OwnerId = ownerId,
        }, cancellationToken));
    
    [HttpGet()]
    [SwaggerOperation(Summary = "Get restaurant's details.")]
    [SwaggerResponse(200, "", typeof(RestaurantDetailsDto))]
    [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
    public async Task<IActionResult> GetRestaurantDetails([FromQuery] int restaurantId, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new RestaurantDetailsQuery(restaurantId), cancellationToken));
}