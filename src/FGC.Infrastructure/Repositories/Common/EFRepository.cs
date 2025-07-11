using FGC.Core.Common.Entities;
using FGC.Core.Common.Repositories.Interfaces;
using FGC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FGC.Infrastructure.Repositories.Common;

public class EfRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : EntityBase<TId> 
{
    protected ApplicationDBContext _appDbContext;
    protected DbSet<TEntity> _dbSet;

    public EfRepository(ApplicationDBContext context)
    {
        _appDbContext = context;
        _dbSet = _appDbContext.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync()
    => await _dbSet.ToListAsync();

    public async Task<TEntity?> GetByIdAsync(TId id)
     => await _dbSet.FindAsync(id);


    public async Task RemoveAsync(TId id)
    {
        var ent = await _dbSet.FindAsync(id);
        if (ent != null)
        {
            _dbSet.Remove(ent);
            await _appDbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _appDbContext.SaveChangesAsync();
    }

}

