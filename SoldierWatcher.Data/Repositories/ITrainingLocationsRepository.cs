using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Data.Repositories;

public interface ITrainingLocationsRepository
{
    Task<TrainingLocation?> GetTrainingLocationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrainingLocation>> GetTrainingLocationsAsync(CancellationToken cancellationToken = default);
}
