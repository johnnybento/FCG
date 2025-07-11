using FGC.Core.Sale.Entities;
using FGC.Core.Sale.Repositories;
using FGC.Infrastructure.Data;
using FGC.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace FGC.Infrastructure.Repositories.Sales;

public class JogoAdquiridoRepository : EfRepository<JogoAdquirido, Guid>, IJogoAdquiridoRepository
{

    public JogoAdquiridoRepository(ApplicationDBContext ctx)
: base(ctx) { }
    public async Task<bool> ExistsAsync(Guid usuarioId, Guid jogoId)
        => await _dbSet.AnyAsync(ja => ja.UsuarioId == usuarioId && ja.JogoId == jogoId);

    public async Task<IEnumerable<JogoAdquirido>> GetByUsuarioIdAsync(Guid usuarioId)
        => await _dbSet.Where(ja => ja.UsuarioId == usuarioId)
                          .OrderByDescending(ja => ja.DataHora)
                          .ToListAsync();
}
