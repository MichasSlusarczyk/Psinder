using MediatR;
using Psinder.API.Domain.Models.Pets;
using Psinder.DB.Common.Repositories.Files;
using Psinder.DB.Domain.Entities;
using Psinder.DB.Domain.Repositories.Pets;
using static Psinder.API.Domain.Models.Pets.AddPetRequest;

namespace Psinder.API.Domain.Handlers.Pets;

public class AddPetRequestHandler : IRequestHandler<AddPetMediatr, AddPetResponse>
{
    private readonly IFileRepository _fileRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<AddPetRequestHandler> _logger;

    public AddPetRequestHandler(
        IFileRepository fileRepository,
        IPetRepository petRepository,
        ILogger<AddPetRequestHandler> logger)
    {
        _fileRepository = fileRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<AddPetResponse> Handle(AddPetMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var domainModel = AddPetRequest.ToDomain(request);

            foreach(var file in request.Attachments)
            {
                var fileDomain = await DB.Common.Entities.File.ToDomain(file);

                var fileId = await _fileRepository.AddFile(fileDomain, cancellationToken);

                domainModel.PetImages!.Add(new PetImage() { FileId = fileId });
            }

            var petId = await _petRepository.AddPet(domainModel, cancellationToken);

            return new AddPetResponse() { PetId = petId };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}