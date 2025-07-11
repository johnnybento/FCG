

using FGC.Application.Common.Ports;
using FGC.Core.Users.Events;
using MediatR;

namespace FGC.Application.Events;

public class UsuarioDesativadoHandler : INotificationHandler<DomainEventNotification<UsuarioDesativadoEvent>>
{

    private readonly IEmailSender _email;

    public UsuarioDesativadoHandler(IEmailSender email) => _email = email;
    public Task Handle(DomainEventNotification<UsuarioDesativadoEvent> notification, CancellationToken cancellationToken)
    {
        //Envia email de notificação para o usuário informando que a conta foi desativada.
        var user = notification.Event.Usuario;
        return Task.CompletedTask;
    }
}
