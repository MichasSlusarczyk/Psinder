using Microsoft.IdentityModel.Tokens;
using Psinder.API.Domain.Handlers.Pets.GetListsSqlBuilder;
using Psinder.Db.Domain.Models.Pets;
using Psinder.DB.Common.Extensions;
using Psinder.DB.Common.SqlBuilders;

namespace Psinder.DB.Domain.Repositories.Pets;

public class PetsListSqlBuilder : BaseSearchSqsBuilder<GetPetsListFilters, PetsListSortColumns>, IPetsListSqlBuilder
{
    public PetsListSqlBuilder() : base("pets", "pets.id")
    {
    }

    public override ISearchSqlBuilder<GetPetsListFilters, PetsListSortColumns> AddSelect()
    {
        _sqlBuilder
            .Select($@"
            pets.id
            ");

        return this;
    }

    public override ISearchSqlBuilder<GetPetsListFilters, PetsListSortColumns> AddJoins()
    {
        _sqlBuilder
            .Join("shelters s ON pets.shelter_id = s.id");

        return this;
    }

    public override ISearchSqlBuilder<GetPetsListFilters, PetsListSortColumns> AddFilters(GetPetsListFilters filters)
    {
        if (filters.ShelterId != null)
        {
            _sqlBuilder.Where($"pets.shelter_id = {filters.ShelterId}");
        }

        if (!filters.Cities.IsNullOrEmpty())
        {
            filters.Cities = filters.Cities!.Select(x => $"\'{x}\'").ToList();
            var cities = string.Join(",", filters.Cities!);
            _sqlBuilder.Where($"s.city IN ({cities})");
        }

        if (!filters.Name.IsNullOrEmpty())
        {
            _sqlBuilder.Where($"pets.name LIKE '%{filters.Name}%' OR pets.name LIKE '%{filters.Name}%'");
        }

        if (!filters.Description.IsNullOrEmpty())
        {
            _sqlBuilder.Where($"pets.description LIKE '%{filters.Description}%' OR pets.description LIKE '%{filters.Description}%'");
        }

        if (!filters.Breed.IsNullOrEmpty())
        {
            _sqlBuilder.Where($"pets.breed LIKE '%{filters.Breed}%' OR pets.breed LIKE '%{filters.Breed}%'");
        }

        if (filters.YearOfBirth != null)
        {
            _sqlBuilder.Where($"pets.year_of_birth = {filters.YearOfBirth.ToInsertString()}");
        }

        if (!filters.Number.IsNullOrEmpty())
        {
            _sqlBuilder.Where($"pets.number LIKE '%{filters.Number}%' OR pets.number LIKE '%{filters.Number}%'");
        }

        if (filters.Gender != null)
        {
            _sqlBuilder.Where($"pets.gender = {filters.Gender.IntToInsertString()}");
        }

        if (filters.Size != null)
        {
            _sqlBuilder.Where($"pets.size = {filters.Size.IntToInsertString()}");
        }

        if (filters.PhysicalActivity != null)
        {
            _sqlBuilder.Where($"pets.physical_activity = {filters.PhysicalActivity.IntToInsertString()}");
        }

        if (filters.AttitudeTowardsChildren != null)
        {
            _sqlBuilder.Where($"pets.attitude_towards_children = {filters.AttitudeTowardsChildren.IntToInsertString()}");
        }

        if (filters.AttitudeTowardsOtherDogs != null)
        {
            _sqlBuilder.Where($"pets.attitude_towards_other_dogs = {filters.AttitudeTowardsOtherDogs.IntToInsertString()}");
        }

        return this;
    }
}
