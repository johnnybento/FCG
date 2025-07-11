using FGC.Core.Catalog.Entities;
using FGC.Core.Common.Repositories.Interfaces;

namespace FGC.Core.Catalog.Repositories;


/// <summary>
/// Operações específicas de promoção.
/// </summary>
public interface IPromocaoRepository : IRepository<Promocao, Guid>
{
    /// <summary>
    /// Retorna todas as promoções ativas de um jogo numa dada data.
    /// </summary>
    Task<IEnumerable<Promocao>> GetActiveByJogoIdAsync(Guid jogoId, DateTime dataReferencia);
}
