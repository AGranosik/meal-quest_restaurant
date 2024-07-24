using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webapi.Controllers.Restaurants.Requests;

namespace webapi.Controllers.Restaurants
{
    public class RestaurantController(IMediator mediator) : ApiController(mediator)
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Create Resutarant.")]
        [SwaggerResponse(200, "", typeof(Result<RestaurantId>))]
        [SwaggerResponse(400, "Some error occured. Check logs with provided requestId.", typeof(Result))]
        public async Task<IActionResult> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request.CastoCommand(), cancellationToken);
            return MapApplicationResultToHttpResponse(result);
        }
    }
}
