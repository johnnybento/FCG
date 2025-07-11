

using FGC.Application.Common.Ports;
using FGC.Core.Catalog.Events;
using FGC.Core.Sale.Events;
using FGC.Core.Users.Events;
using MediatR;

namespace FGC.Application.Events;

public class JogoAdquiridoNaBibliotecaHandler : INotificationHandler<DomainEventNotification<JogoAdquiridoNaBibliotecaEvent>>
{
    private readonly IEmailSender _email;

    public JogoAdquiridoNaBibliotecaHandler(IEmailSender email) => _email = email;

    public Task Handle(DomainEventNotification<JogoAdquiridoNaBibliotecaEvent> notification, CancellationToken cancellationToken)
    {
        //Envia email de notificação para o usuário informando o novo jogo adicionado a biblioteca.
        var user = notification.Event.Compra;
        return Task.CompletedTask;   
    }
}
