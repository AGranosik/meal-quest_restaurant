using MediatR;
using Microsoft.AspNetCore.Mvc;
using webapi.Controllers.Restaurants.Requests;

namespace webapi.Controllers.Restaurants
{
    public class RestaurantController(IMediator mediator) : ApiController(mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateRestaurantAsync(CreateRestaurantRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request.CastoCommand(), cancellationToken);
            return MapApplicationResultToHttpResponse(result);
        }
    }
}
