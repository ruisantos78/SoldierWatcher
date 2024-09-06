using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.MemoryCache.Core;
using SoldierWatcher.Data.MemoryCache.Entities;
using SoldierWatcher.Data.Repositories;

namespace SoldierWatcher.Data.MemoryCache.Repositories;

internal class SoldiersRepository: ISoldiersRepository
{
    private readonly IDataContext dataContext;
    private readonly ICountriesRepository countriesRepository;

    public SoldiersRepository(IDataContext dataContext,
        ICountriesRepository countriesRepository)
    {
        this.dataContext = dataContext;
        this.countriesRepository = countriesRepository;
    }

    public async Task<Soldier?> GetSoldierByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var soldiers = await GetSoldiersEnititesAsync(cancellationToken);

        if (soldiers.FirstOrDefault(x => x.Id == id) is { } soldier)
            return await ToSoldierAsync(soldier, cancellationToken);

        return default;
    }

    private Task<SoldierEntity[]> GetSoldiersEnititesAsync(CancellationToken cancellationToken)
    {
        return dataContext.GetEntitiesAsync<SoldierEntity>("Soldiers", cancellationToken);
    }

    private async Task<Soldier> ToSoldierAsync(SoldierEntity entity, CancellationToken cancellationToken) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Rank = entity.Rank,
        Country = await countriesRepository.GetCountryByIdAsync(entity.CountryId, cancellationToken) ?? Country.Empty
    };
}
