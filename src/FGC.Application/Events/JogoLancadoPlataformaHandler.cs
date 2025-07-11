using FGC.Application.Common.Ports;
using FGC.Core.Catalog.Events;
using FGC.Core.Users.Events;
using MediatR;

namespace FGC.Application.Events;

public class JogoLancadoPlataformaHandler : INotificationHandler<DomainEventNotification<JogoCadastradoEvent>>
{
    private readonly IEmailSender _email;

    public JogoLancadoPlataformaHandler(IEmailSender email) => _email = email;

    public Task Handle(DomainEventNotification<JogoCadastradoEvent> notification, CancellationToken cancellationToken)
    {
        
        var jogo = notification.Event.Jogo;
        return Task.CompletedTask;        
    }
}
