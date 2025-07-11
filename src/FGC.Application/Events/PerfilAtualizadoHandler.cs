using FGC.Application.Common.Ports;
using FGC.Core.Users.Events;
using MediatR;

namespace FGC.Application.Events;

public class PerfilAtualizadoHandler : INotificationHandler<DomainEventNotification<PerfilAtualizadoEvent>>
{
    private readonly IEmailSender _email;

    public PerfilAtualizadoHandler(IEmailSender email) => _email = email;

    public Task Handle(DomainEventNotification<PerfilAtualizadoEvent> notification, CancellationToken cancellationToken)
    {
        //Envia email de notificação para o usuário informando que o perfil foi atualizado.
        var user = notification.Event.Usuario;
        return Task.CompletedTask;
    }
}
