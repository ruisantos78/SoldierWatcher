using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.MemoryCache.Core;
using SoldierWatcher.Data.MemoryCache.Entities;
using SoldierWatcher.Data.Repositories;

namespace SoldierWatcher.Data.MemoryCache.Repositories;

internal class TrainingLocationsRepository : ITrainingLocationsRepository
{
    private readonly IDataContext dataContext;

    public TrainingLocationsRepository(IDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<TrainingLocation?> GetTrainingLocationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entities = await GetTrainingLocationsEntitiesAsync(cancellationToken);
        
        if (entities.FirstOrDefault(x => x.Id == id) is { } entity)
            return ToTrainingLocation(entity);

        return null;
    }

    public async Task<IReadOnlyList<TrainingLocation>> GetTrainingLocationsAsync(CancellationToken cancellationToken = default)
    {
        var entities = await GetTrainingLocationsEntitiesAsync(cancellationToken);
        return entities.Select(ToTrainingLocation).ToArray();
    }

    private Task<TrainingLocationEntity[]> GetTrainingLocationsEntitiesAsync(CancellationToken cancellationToken)
    {
        return dataContext.GetEntitiesAsync<TrainingLocationEntity>("TrainingLocations", cancellationToken);
    }

    private TrainingLocation ToTrainingLocation(TrainingLocationEntity entity) => new()
    {
        Id = entity.Id,
        LocationName = entity.LocationName,
        Latitude = entity.Latitude,
        Longitude = entity.Longitude
    };
}
