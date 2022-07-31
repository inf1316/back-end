using MediatR;
using QvaCar.Application.Exceptions;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Services;
using QvaCar.Seedwork.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.CarAds
{
    public class AddImagesToCarAdCommandHandler : IRequestHandler<AddImagesToCarAdCommand>
    {
        private readonly ICarAdRepository _carAdRepository;
        private readonly IImageService _imageService;
        private readonly IFileSystemService _fileSystemService;
        private readonly ICurrentUserService _currentUserService;

        public AddImagesToCarAdCommandHandler
         (
             ICarAdRepository carAdRepository,
             IImageService imageService,
             IFileSystemService fileSystemService,
             ICurrentUserService currentUserService
         )
        {
            _carAdRepository = carAdRepository;
            _imageService = imageService;
            _fileSystemService = fileSystemService;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(AddImagesToCarAdCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetCurrentUser().Id;           

            var car = await _carAdRepository.GetByUserAndAdIdsAsync(userId, command.Id, cancellationToken);
            Check.IsNotNull<EntityNotFoundException>(car, $"Entity with Id { command.Id } not found.");

            var filesNamesToAdd = _fileSystemService.ResolveFileNameConflicts(car.Images.Select(x => x.FileName).ToArray(), command.Images.Select(x => x.FileName).ToArray());
            car.AddImages(filesNamesToAdd.ToArray());

            for (int i = 0; i < command.Images.Length; i++)
            {
                var image = command.Images[i];
                var saveFileName = filesNamesToAdd[i];
                await _imageService.SaveImageAsync(userId, command.Id, saveFileName, image.File, cancellationToken);
            }

            await _carAdRepository.UpdateAsync(car, cancellationToken);

            return Unit.Value;
        }
    }
}
