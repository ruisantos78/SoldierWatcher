using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.MemoryCache.Core;
using SoldierWatcher.Data.MemoryCache.Entities;
using SoldierWatcher.Data.Repositories;

namespace SoldierWatcher.Data.MemoryCache.Repositories;

internal class CountriesRepository : ICountriesRepository
{
    private readonly IDataContext dataContext;

    public CountriesRepository(IDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {        
        var countries = await GetCountriesEnititesAsync(cancellationToken);

        if (countries.FirstOrDefault(x => x.Id == id) is { } country)
            return ToCountry(country);  

        return null;
    }

    private Task<CountryEntity[]> GetCountriesEnititesAsync(CancellationToken cancellationToken)
    {
        return dataContext.GetEntitiesAsync<CountryEntity>("Countries", cancellationToken);
    }

    private static Country ToCountry(CountryEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };
}
