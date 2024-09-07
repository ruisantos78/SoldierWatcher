using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.MemoryCache.Core;
using SoldierWatcher.Data.MemoryCache.Entities;
using SoldierWatcher.Data.Repositories;

namespace SoldierWatcher.Data.MemoryCache.Repositories;

internal class SoldierMarkerRepository : ISoldierMarkerRepository
{
    private readonly IDataContext dataContext;
    private readonly ISensorsRepository sensorsRepository;
    private readonly ISoldiersRepository soldiersRepository;
    private readonly ITrainingLocationsRepository trainingLocationsRepository;

    public SoldierMarkerRepository(IDataContext dataContext,
        ISensorsRepository sensorsRepository,
        ISoldiersRepository soldiersRepository,
        ITrainingLocationsRepository trainingLocationsRepository)
    {
        this.dataContext = dataContext;
        this.sensorsRepository = sensorsRepository;
        this.soldiersRepository = soldiersRepository;
        this.trainingLocationsRepository = trainingLocationsRepository;
    }

    public async Task UpdateSoldierLocationAsync(SoldierMarker soldierMarker, CancellationToken cancellationToken = default)
    {
        await File.AppendAllTextAsync(Path.Combine(Environment.CurrentDirectory, $"{soldierMarker.Id}.log"), 
            $"[{DateTime.Now}] {soldierMarker.Latitude}, {soldierMarker.Longitude}\n", cancellationToken);
    }

    public async Task<IReadOnlyList<SoldierMarker>> GetSoldiersByTrainingLocationAsync(TrainingLocation trainingLocation, CancellationToken cancellationToken = default)
    {
        var entities = await GetTrainingLocationsEntitiesAsync(cancellationToken);

        var converterTasks = entities.Where(x => x.TrainingLocationId == trainingLocation.Id)
            .Select(x => ToSoldierMarkerAsync(x, cancellationToken));

        return await Task.WhenAll(converterTasks);
    }

    private Task<SoldierMarkerEntity[]> GetTrainingLocationsEntitiesAsync(CancellationToken cancellationToken)
    {
        return dataContext.GetEntitiesAsync<SoldierMarkerEntity>("SoldierMarkers", cancellationToken);
    }

    private async Task<SoldierMarker> ToSoldierMarkerAsync(SoldierMarkerEntity entity, CancellationToken cancellationToken = default) => new()
    {
        Id = entity.Id,
        Sensor = await sensorsRepository.GetSensorByIdAsync(entity.SensorId, cancellationToken) ?? Sensor.Empty,
        Soldier = await soldiersRepository.GetSoldierByIdAsync(entity.SoldierId, cancellationToken) ?? Soldier.Empty,
        TrainingLocation = await trainingLocationsRepository.GetTrainingLocationByIdAsync(entity.TrainingLocationId, cancellationToken) ?? TrainingLocation.Empty,

        TrainingInfo = entity.TrainingInfo,
        Latitude = entity.Latitude,
        Longitude = entity.Longitude,
        SerialNumber = entity.SerialNumber
    };
}
