using MediatR;
using Microsoft.IdentityModel.Tokens;
using Psinder.API.Common;
using Psinder.API.Common.Exceptions;
using Psinder.API.Domain.Models.Pets;
using Psinder.DB.Common.Repositories.Files;
using Psinder.DB.Domain.Entities;
using Psinder.DB.Domain.Repositories.Pets;

namespace Psinder.API.Domain.Handlers.Pets;

public class UpdatePetRequestHandler : IRequestHandler<UpdatePetMediatr, Unit>
{
    private readonly IFileRepository _fileRepository;
    private readonly IPetRepository _petRepository;
    private readonly IIdentityService _identityService;

    private readonly ILogger<UpdatePetRequestHandler> _logger;

    public UpdatePetRequestHandler(
        IFileRepository fileRepository,
        IPetRepository petRepository,
        IIdentityService identityService,
        ILogger<UpdatePetRequestHandler> logger)
    {
        _fileRepository = fileRepository;
        _petRepository = petRepository;
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdatePetMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            if (!_identityService.VerifyAccessForWorker(request.Content.Id))
            {
                throw new AccessDeniedException($"For a logged in user with role: {_identityService.CurrentUserRole} and ID: {_identityService.CurrentUserId} no access to data of pet with ID: {request.Content.Id}.");
            }

            var petData = await _petRepository.GetPet(request.Content.Id, cancellationToken)
                ?? throw new Exception($"No user data found for ID: {request.Content.Id}.");

            var updatedData = UpdatePetData(petData, request);
            await _petRepository.UpdatePet(petData, cancellationToken);

            await UpdatePetImages(petData, request, cancellationToken);
            await UpdatePetTraits(petData, request, cancellationToken);

            return new Unit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private static Pet UpdatePetData(Pet data, UpdatePetRequest request)
    {
        data.PhysicalActivity = request.Content.PhysicalActivity ?? data.PhysicalActivity;
        data.AttitudeTowardsChildren = request.Content.AttitudeTowardsChildren ?? data.AttitudeTowardsChildren;
        data.AttitudeTowardsOtherDogs = request.Content.AttitudeTowardsOtherDogs ?? data.AttitudeTowardsOtherDogs;
        data.Breed = request.Content.Breed ?? data.Breed;
        data.Description = request.Content.Description ?? data.Description;
        data.Gender = request.Content.Gender ?? data.Gender;
        data.Name = request.Content.Name ?? data.Name;
        data.Number = request.Content.Number ?? data.Number;
        data.YearOfBirth = request.Content.YearOfBirth ?? data.YearOfBirth;
        data.Size = request.Content.Size ?? data.Size;

        return data;
    }

    private async Task UpdatePetImages(Pet petData, UpdatePetRequest request, CancellationToken cancellationToken)
    {
        if (!request.AttachmentsToAdd.IsNullOrEmpty())
        {
            foreach (var image in request.AttachmentsToAdd!)
            {
                var file = await DB.Common.Entities.File.ToDomain(image);
                var fileId = await _fileRepository.AddFile(file, cancellationToken);
                await _petRepository.AddPetImage(new PetImage() { FileId = fileId, PetId = petData.Id }, cancellationToken);
            }
        }

        if (!request.AttachmentsToDelete.IsNullOrEmpty())
        {
            foreach (var fileId in request.AttachmentsToDelete!)
            {
                await _petRepository.DeletePetImage(new PetImage() { FileId = fileId, PetId = petData.Id }, cancellationToken);
                await _fileRepository.DeleteFileById(fileId, cancellationToken);
            }
        }
    }

    private async Task UpdatePetTraits(Pet petData, UpdatePetRequest request, CancellationToken cancellationToken)
    {
        if (!request.Content.PetTraitsToAdd.IsNullOrEmpty())
        {
            foreach (var petTrait in request.Content.PetTraitsToAdd!)
            {
                await _petRepository.AddPetTrait(new PetTrait() { TraitId = (byte)petTrait, PetId = petData.Id }, cancellationToken);
            }
        }

        if (!request.Content.PetTraitsToDelete.IsNullOrEmpty())
        {
            foreach (var petTrait in request.Content.PetTraitsToDelete!)
            {
                await _petRepository.DeletePetTrait(new PetTrait() { TraitId = (byte)petTrait, PetId = petData.Id }, cancellationToken);
            }
        }
    }
}