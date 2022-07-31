using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QvaCar.Api.Features.CarAds.Requests;
using QvaCar.Application.Features.CarAds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Api.Features.CarAds
{
    [Route("api/car-ads")]
    [ApiVersion("1.0")]
    public class CarAdController : AuthorizeApiControllerBase
    {
        [HttpGet]       
        [Route("")]        
        public async Task<CarAdsResponse> GetAdsAsync(CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new GetCarAdsByUserCommand(), cancellationToken);
            return Mapper.Map<CarAdsResponse>(response);
        }

        [HttpGet]
        [Route("{id}")]        
        [ActionName(nameof(GetAdByIdAsync))]
        public async Task<ActionResult<CarAdByIdResponse>> GetAdByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetCarAdByIdCommand() { Id = id };
            var response = await Mediator.Send(command, cancellationToken);            
            return Mapper.Map<CarAdByIdResponse>(response);
        }

        [HttpPost]
        [Route("register")]        
        public async Task<ActionResult<RegisterCarAdResponse>> RegisterAdAsync([FromBody] RegisterCarAdRequest request, CancellationToken cancellationToken)
        {
            var command = Mapper.Map<RegisterCarAdCommand>(request);
            var id = await Mediator.Send(command, cancellationToken);
            var getByIdActionsParam = new { id };
            return CreatedAtAction(nameof(GetAdByIdAsync), getByIdActionsParam, new RegisterCarAdResponse() { Id = id.ToString() });
        }

        [HttpDelete]
        [Route("{id}/unregister")]
        public async Task<ActionResult> UnregisterAdAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new UnregisterCarAdByIdCommand() { Id = id };
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}/publish")]
        public async Task<ActionResult> PublishAdAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new PublishCarAdByIdCommand() { Id = id };
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAdAsync(
                [FromRoute] Guid id,
                [FromBody] UpdateCarAdRequest request,
                CancellationToken cancellationToken)
        {
            var command = Mapper.Map<UpdateCarAdCommand>(request) with { Id = id };
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("reference-data")]
        public async Task<ActionResult<CarAdReferenceDataResponse>> GetReferenceDataAsync(CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new GetCarAdsReferenceDataCommand(), cancellationToken);
            return Mapper.Map<CarAdReferenceDataResponse>(response);
        }

        [HttpPost]
        [Route("{id}/images/upload")]
        public async Task<IActionResult> AddImage([FromRoute] Guid id, [FromForm] AddImagesToCarAdRequest request,
                CancellationToken cancellationToken)
        {
            var command = Mapper.Map<AddImagesToCarAdCommand>(request) with { Id = id };

            await Mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}