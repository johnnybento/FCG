namespace FGC.Core.Common.Entities.Interfaces;
/// <summary>
/// Interface genérica para toda entidade com chave.
/// </summary>
public interface IEntity<TId>
{
    TId Id { get; }
}
