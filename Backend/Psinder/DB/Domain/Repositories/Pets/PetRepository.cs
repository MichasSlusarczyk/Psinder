using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Psinder.Db.Domain.Models.Pets;
using Psinder.DB.Common.Repositories.UnitOfWorks;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Domain.Repositories.Pets;

public class PetRepository : IPetRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public PetRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long> AddPet(Pet pet, CancellationToken cancellationToken)
    {
        await _unitOfWork.DatabaseContext.PetsEntity.AddAsync(pet);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();

        return pet.Id;
    }

    public async Task UpdatePet(Pet pet, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.PetsEntity.Update(pet);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<Pet?> GetPet(long petId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.PetsEntity
            .Include(x => x.PetTraits).ThenInclude(x => x.Trait)
            .Include(x => x.PetImages).ThenInclude(x => x.File)
            .SingleOrDefaultAsync(x => x.Id == petId, cancellationToken);
    }

    public async Task<GetPetsListResponse> GetPets(GetPetsListMediatr request, CancellationToken cancellationToken)
    {
        var builder = new PetsListSqlBuilder();
        var query = builder
            .AddSelect()
            .AddFrom()
            .AddJoins()
            .AddFilters(request.Filters)
            .AddTotalSizeQuerySelect()
            .BuildSelectQuery();
        
        var result = await _unitOfWork.Connection.QueryAsync<GetPetsListRowResponse>(
            query.RawSql,
            query.Parameters);

        var totalSizeQuery = builder.BuildTotalSizeQuery();
        var totalSize = await _unitOfWork.Connection.ExecuteScalarAsync<int>(totalSizeQuery.RawSql, query.Parameters);

        return new GetPetsListResponse()
        {
            PageSize = request.Paging.PageSize,
            TotalSize = totalSize,
            List = result != null ? result.ToList() : new List<GetPetsListRowResponse>()
        };
    }

    public async Task DeletePet(Pet pet, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.FilesEntity
            .RemoveRange(pet.PetImages!.Select(x => x.File));

        _unitOfWork.DatabaseContext.PetImagesEntity
            .RemoveRange(pet.PetImages!);

        _unitOfWork.DatabaseContext.PetsEntity.Remove(pet);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<List<PetTraits>?> GetPetTraitsByPetId(long petId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.PetTraitsEntity
            .Where(x => x.PetId == petId)
            .Select(x => (PetTraits)x.TraitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<GetPetsListRowResponse.GetPetsListRowResponseAttachment>?> GetPetImagesByPetId(long petId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.PetImagesEntity.Include(x => x.File)
            .Where(x => x.PetId == petId)
            .Select(x => new GetPetsListRowResponse.GetPetsListRowResponseAttachment()
            {
                Id = x.File.Id,
                Name = x.File.Name,
                Extension = x.File.Extension,
                ContentType = x.File.ContentType,
                ContentLength = x.File.ContentLength
            })
            .ToListAsync(cancellationToken);
    }

    public async Task AddPetImage(PetImage petImage, CancellationToken cancellationToken)
    {
        await _unitOfWork.DatabaseContext.PetImagesEntity.AddAsync(petImage);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task DeletePetImage(PetImage petImage, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.PetImagesEntity.Remove(petImage);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task AddPetTrait(PetTrait petTrait, CancellationToken cancellationToken)
    {
        await _unitOfWork.DatabaseContext.PetTraitsEntity.AddAsync(petTrait);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task DeletePetTrait(PetTrait petTrait, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.PetTraitsEntity.Remove(petTrait);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }
}