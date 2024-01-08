using MediatR;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Domain.Entities;
using Psinder.DB.Domain.Repositories.Shelters;

namespace Psinder.API.Domain.Handlers;

public class UpdateShelterRequestHandler : IRequestHandler<UpdateShelterMediatr, UpdateShelterResponse>
{
    private readonly IShelterRepository _shelterRepository;
    private readonly ILogger<UpdateShelterRequestHandler> _logger;

    public UpdateShelterRequestHandler(
        IShelterRepository shelterRepository,
        ILogger<UpdateShelterRequestHandler> logger)
    {
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<UpdateShelterResponse> Handle(UpdateShelterMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var userData = await _shelterRepository.GetShelterById(request.Id, cancellationToken)
                ?? throw new Exception($"No user data found for ID: {request.Id}.");

            var updatedData = UpdateData(userData, request);
            await _shelterRepository.UpdateShelter(userData, cancellationToken);

            return UpdateShelterResponse.FromDomain(updatedData);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private static Shelter UpdateData(Shelter data, UpdateShelterRequest request)
    {
        data.Description = request.Description ?? data.Description;
        data.City = request.City ?? data.City;
        data.Address = request.Address ?? data.Address;
        data.Email = request.Email ?? data.Email;
        data.Name = request.Name ?? data.Name;
        data.PhoneNumber = request.PhoneNumber ?? data.PhoneNumber;

        return data;
    }
}