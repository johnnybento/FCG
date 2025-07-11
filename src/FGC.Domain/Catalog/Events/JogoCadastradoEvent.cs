using FGC.Core.Catalog.Entities;
using FGC.Core.Common.Events;

namespace FGC.Core.Catalog.Events;

public sealed class JogoCadastradoEvent : DomainEvent
{
    public Jogo Jogo { get; }

    public JogoCadastradoEvent(Jogo jogo)
        => Jogo = jogo;
}