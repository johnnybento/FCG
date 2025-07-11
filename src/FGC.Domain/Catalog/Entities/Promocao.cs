using FGC.Core.Catalog.Events;
using FGC.Core.Common.Entities;
using FGC.Core.Common.Entities.Interfaces;
using FGC.Core.Exceptions;

namespace FGC.Core.Catalog.Entities;

public class Promocao : EntityBase<Guid>, IAggregateRoot
{
    public Guid JogoId { get; private set; }
    public decimal Desconto { get; private set; }
    public DateTime Inicio { get; private set; }
    public DateTime Termino { get; private set; }

    private Promocao() { }

    public Promocao(Guid jogoId, decimal desconto, DateTime inicio, DateTime termino)
    {
        if (desconto <= 0 || desconto > 100)
            throw new DomainException("Desconto deve ser entre 1 e 100%.");

        if (inicio >= termino || termino < DateTime.UtcNow.Date)
            throw new DomainException("Período de promoção inválido.");


        Id = Guid.NewGuid();
        JogoId = jogoId;
        Desconto = desconto;
        Inicio = inicio;
        Termino = termino;


        Common.Events.DomainEvents.Raise(new PromocaoCriadaEvent(this));
    }
    public bool EstaAtiva(DateTime agora) =>
          agora >= Inicio && agora <= Termino;

}
