using FGC.Core.Catalog.Entities;
using FGC.Core.Common.Events;

namespace FGC.Core.Catalog.Events;

public sealed class PromocaoCriadaEvent : DomainEvent
{
    public Promocao Promocao { get; }

    public PromocaoCriadaEvent(Promocao promocao)
        => Promocao = promocao;
}