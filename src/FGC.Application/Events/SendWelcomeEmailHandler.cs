using FGC.Application.Common.Ports;
using FGC.Core.Users.Events;
using MediatR;

namespace FGC.Application.Events;

public class SendWelcomeEmailHandler : INotificationHandler<DomainEventNotification<UsuarioCadastradoEvent>>
{
    private readonly IEmailSender _email;

    public SendWelcomeEmailHandler(IEmailSender email) => _email = email;

    public Task Handle(DomainEventNotification<UsuarioCadastradoEvent> notification, CancellationToken cancellationToken)
    {
        var user = notification.Event.Usuario;
        return Task.CompletedTask;
   
    }
}
