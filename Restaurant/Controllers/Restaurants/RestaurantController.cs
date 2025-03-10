using application.Restaurants.Queries.GetRestaurantQueries;
using application.Restaurants.Queries.GetRestaurantQueries.Dtos;
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
    [SwaggerOperation(Summary = "Create Resutarant.")]
    [SwaggerResponse(200, "", typeof(Result<RestaurantId>))]
    [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
    public async Task<IActionResult> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.CastoCommand(), cancellationToken);
        return MapApplicationResultToHttpResponse(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get owner's resutaurants.")]
    [SwaggerResponse(200, "", typeof(List<RestaurantDto>))]
    [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
    public async Task<IActionResult> GetOwnersRestaurantAsync([FromQuery] int ownerId, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetRestaurantQuery
        {
            OwnerId = ownerId,
        }, cancellationToken));
}