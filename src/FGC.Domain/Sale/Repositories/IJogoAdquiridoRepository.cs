
using FGC.Core.Common.Repositories.Interfaces;
using FGC.Core.Sale.Entities;

namespace FGC.Core.Sale.Repositories;

/// <summary>
/// Operações para registrar e consultar compras de jogos.
/// </summary>
public interface IJogoAdquiridoRepository : IRepository<JogoAdquirido, Guid>
{
    /// <summary>
    /// Verifica se o usuário já adquiriu este jogo.
    /// </summary>
    Task<bool> ExistsAsync(Guid usuarioId, Guid jogoId);

    /// <summary>
    /// Recupera todas as compras de um usuário (biblioteca).
    /// </summary>
    Task<IEnumerable<JogoAdquirido>> GetByUsuarioIdAsync(Guid usuarioId);
}