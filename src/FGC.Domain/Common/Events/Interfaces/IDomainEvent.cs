
namespace FGC.Core.Common.Events.Interfaces;

/// <summary>
/// Marca um evento de domínio.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
