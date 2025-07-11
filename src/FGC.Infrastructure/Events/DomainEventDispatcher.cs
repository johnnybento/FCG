
using FGC.Core.Common.Events;
using FGC.Application.Events;
using FGC.Core.Common.Events.Interfaces;
using MediatR;

namespace FGC.Infrastructure.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
        => _mediator = mediator;

    public async Task DispatchAsync()
    {
       
        var events = DomainEvents.Events.ToArray();
        

        foreach (var @event in events)
        {
            
            var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(@event.GetType());
            var wrapper = Activator.CreateInstance(wrapperType, @event)
                      as INotification
                  ?? throw new InvalidOperationException("não pôde criar wrapper de evento");

            await _mediator.Publish(wrapper);
        }
        DomainEvents.Clear();
    }
}
