using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Data.Repositories;

public interface ISoldierMarkerRepository
{
    Task<IReadOnlyList<SoldierMarker>> GetSoldiersByTrainingLocation(TrainingLocation trainingLocation, CancellationToken cancellationToken = default);
    Task UpdateSoldierLocation(SoldierMarker soldierMarker, CancellationToken cancellationToken = default);
}
