using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Data.Repositories;

public interface ISoldierMarkerRepository
{
    Task<IReadOnlyList<SoldierMarker>> GetSoldiersByTrainingLocationAsync(TrainingLocation trainingLocation, CancellationToken cancellationToken = default);
    Task UpdateSoldierLocationAsync(SoldierMarker soldierMarker, CancellationToken cancellationToken = default);
}
