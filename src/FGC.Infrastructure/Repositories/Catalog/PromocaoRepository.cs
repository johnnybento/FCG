using FGC.Core.Catalog.Entities;
using FGC.Core.Catalog.Repositories;
using FGC.Infrastructure.Data;
using FGC.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace FGC.Infrastructure.Repositories.Catalog;

public class PromocaoRepository : EfRepository<Promocao, Guid>, IPromocaoRepository
{
    public PromocaoRepository(ApplicationDBContext ctx)
  : base(ctx) { }


    public async Task<IEnumerable<Promocao>> GetActiveByJogoIdAsync(Guid jogoId, DateTime dataReferencia)
    => await _dbSet.Where(p => p.JogoId == jogoId && dataReferencia >= p.Inicio && dataReferencia <= p.Termino)
                      .ToListAsync();

}
