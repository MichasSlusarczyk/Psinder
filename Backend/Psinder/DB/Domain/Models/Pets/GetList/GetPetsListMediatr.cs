using MediatR;
using Psinder.DB.Common.Searching;

namespace Psinder.Db.Domain.Models.Pets;

public class GetPetsListMediatr : SearchRequest<GetPetsListFilters, PetsListSortColumns>, IRequest<GetPetsListResponse>
{
    public GetPetsListMediatr(
        GetPetsListFilters filter,
        PageInfo? page = null,
        SortInfo<PetsListSortColumns>? sort = null) : base(filter, sort, page)
    {
    }
}