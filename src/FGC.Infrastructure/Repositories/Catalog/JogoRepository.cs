using FGC.Core.Catalog.Entities;
using FGC.Core.Catalog.Repositories;
using FGC.Infrastructure.Data;
using FGC.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;


namespace FGC.Infrastructure.Repositories.Catalog;

public class JogoRepository : EfRepository<Jogo, Guid>, IJogoRepository
{
    public JogoRepository(ApplicationDBContext ctx)
  : base(ctx) { }

    public async Task<bool> TituloExistsAsync(string titulo)
        => await _dbSet.AnyAsync(j => j.Titulo == titulo);

    public async Task<Jogo?> GetByTituloAsync(string titulo)
        => await _dbSet.FirstOrDefaultAsync(j => j.Titulo == titulo);
}
