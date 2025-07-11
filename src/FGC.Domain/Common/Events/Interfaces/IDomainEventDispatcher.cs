namespace FGC.Core.Common.Events.Interfaces;

/// <summary>
/// Abstração para despachar eventos de domínio após a persistência.
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync();
}
