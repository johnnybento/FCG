using FGC.Core.Catalog.Entities;
using FGC.Core.Common.Repositories.Interfaces;

namespace FGC.Core.Catalog.Repositories;

/// <summary>
/// Operações específicas de jogo.
/// </summary>
public interface IJogoRepository : IRepository<Jogo, Guid>
{
    /// <summary>
    /// Verifica se já existe um jogo com este título.
    /// </summary>
    Task<bool> TituloExistsAsync(string titulo);

    /// <summary>
    /// Busca um jogo pelo título (ou null).
    /// </summary>
    Task<Jogo> GetByTituloAsync(string titulo);
}
