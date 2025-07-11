using FGC.Core.Common.Entities.Interfaces;

namespace FGC.Core.Common.Repositories.Interfaces;

public interface IRepository<TEntity, TId> where 
       TEntity : IEntity<TId>
{
    Task<TEntity?> GetByIdAsync(TId id);
    Task<IReadOnlyList<TEntity>> ListAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TId id);
}
