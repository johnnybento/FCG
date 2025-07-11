

using FGC.Application.Common.Ports;
using FGC.Core.Users.Events;
using MediatR;

namespace FGC.Application.Events;

public class SenhaAlteradaHandler : INotificationHandler<DomainEventNotification<SenhaAlteradaEvent>>
{

    private readonly IEmailSender _email;

    public SenhaAlteradaHandler(IEmailSender email) => _email = email;
    public Task Handle(DomainEventNotification<SenhaAlteradaEvent> notification, CancellationToken cancellationToken)
    {
        var user = notification.Event.Usuario;
        return Task.CompletedTask;
    }
}
