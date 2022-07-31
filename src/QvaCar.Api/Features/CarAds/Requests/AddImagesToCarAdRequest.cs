using Microsoft.AspNetCore.Http;
using System;

namespace QvaCar.Api.Features.CarAds.Requests
{
    public class AddImagesToCarAdRequest
    {        
        public IFormFile[] Images { get; init; } = Array.Empty<IFormFile>();
    }
}
