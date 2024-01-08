using MediatR;
using Psinder.API.Common;
using Psinder.API.Common.Exceptions;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Domain.Entities;
using Psinder.DB.Domain.Repositories.Shelters;
using Psinder.DB.Domain.Repositories.Users;

namespace Psinder.API.Domain.Handlers;

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserMediatr, UpdateUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IShelterRepository _shelterRepository;
    private readonly ILogger<UpdateUserRequestHandler> _logger;

    public UpdateUserRequestHandler(
        IIdentityService identityService,
        IUserRepository userRepository,
        IShelterRepository shelterRepository,
        ILogger<UpdateUserRequestHandler> logger)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _shelterRepository = shelterRepository;
        _logger = logger;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            if (!_identityService.VerifyAccessForAdmin(request.Id))
            {
                throw new AccessDeniedException($"For a logged in user with role: {_identityService.CurrentUserRole} and ID: {_identityService.CurrentUserId} no access to data of user with ID: {request.Id}.");
            }

            var userData = await _userRepository.GetUserById(request.Id, cancellationToken)
                ?? throw new Exception($"No user data found for ID: {request.Id}.");

            var updatedData = await UpdateData(userData, request, cancellationToken);
            await _userRepository.UpdateUser(userData, cancellationToken);

            return UpdateUserResponse.FromDomain(updatedData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<User> UpdateData(User data, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        data.LoginData.AccountStatusId = (byte?)request.AccountStatus ?? data.LoginData.AccountStatusId;
        data.SignedForNewsletter = request.SignedForNewsletter ?? data.SignedForNewsletter;
        if(request.Role != null)
        {
            data.LoginData.RoleId = (byte)request.Role.Value;
            data.LoginData.Role = await _userRepository.GetRoleById(data.LoginData.RoleId, cancellationToken);
        }

        if(request.ShelterId != null)
        {
            if (await _shelterRepository.CheckIfShelterExists(request.ShelterId.Value, cancellationToken))
            {
                await _userRepository.DeleteWorkerInAllShelters(data.Id, cancellationToken);
                await _userRepository.AddWorker(new Worker() { ShelterId = request.ShelterId.Value, UserId = data.Id }, cancellationToken);
            }
        }
        else
        {
            await _userRepository.DeleteWorkerInAllShelters(data.Id, cancellationToken);
        }

        if (request.UserDetails != null)
        {
            if (data.UserDetails == null)
            {
                data.UserDetails = new UserDetails()
                {
                    BirthDate = request.UserDetails.BirthDate,
                    City = request.UserDetails.City,
                    FirstName = request.UserDetails.FirstName,
                    Gender = request.UserDetails.Gender,
                    LastName = request.UserDetails.LastName,
                    PhoneNumber = request.UserDetails.PhoneNumber,
                    PostalCode = request.UserDetails.PostalCode,
                    Street = request.UserDetails.Street,
                    StreetNumber = request.UserDetails.StreetNumber
                };
            }
            else
            {
                data.UserDetails.BirthDate = request.UserDetails.BirthDate;
                data.UserDetails.City = request.UserDetails.City;
                data.UserDetails.FirstName = request.UserDetails.FirstName;
                data.UserDetails.Gender = request.UserDetails.Gender;
                data.UserDetails.LastName = request.UserDetails.LastName;
                data.UserDetails.PhoneNumber = request.UserDetails.PhoneNumber;
                data.UserDetails.PostalCode = request.UserDetails.PostalCode;
                data.UserDetails.Street = request.UserDetails.Street;
                data.UserDetails.StreetNumber = request.UserDetails.StreetNumber;
            }
        }

        return data;
    }
}