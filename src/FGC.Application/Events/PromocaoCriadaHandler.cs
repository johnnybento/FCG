

using FGC.Application.Common.Ports;
using FGC.Core.Catalog.Events;
using MediatR;

namespace FGC.Application.Events;

public class PromocaoCriadaHandler
    : INotificationHandler<DomainEventNotification<PromocaoCriadaEvent>>
{
    private readonly IEmailSender _email;

    public PromocaoCriadaHandler(IEmailSender email) => _email = email;

    public Task Handle(DomainEventNotification<PromocaoCriadaEvent> notification, CancellationToken cancellationToken)
    {

        //Envia email de notificação para todos os usuários
        var promo = notification.Event.Promocao;        
        return Task.CompletedTask;
    }
}