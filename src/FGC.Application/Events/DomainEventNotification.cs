

using FGC.Core.Common.Events.Interfaces;
using MediatR;

namespace FGC.Application.Events;

/// <summary>
/// Adapta um IDomainEvent puro ao contrato INotification do MediatR.
/// </summary>
public record DomainEventNotification<TEvent>(TEvent Event)
    : INotification
    where TEvent : IDomainEvent;