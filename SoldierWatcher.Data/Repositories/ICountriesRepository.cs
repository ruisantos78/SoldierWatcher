using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Data.Repositories;

public interface ICountriesRepository
{
    Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
