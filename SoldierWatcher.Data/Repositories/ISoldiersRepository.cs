using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Data.Repositories;

public interface ISoldiersRepository
{
    Task<Soldier?> GetSoldierByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
