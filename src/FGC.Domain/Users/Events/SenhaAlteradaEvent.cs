using FGC.Core.Common.Events;
using FGC.Core.Users.Entities;

namespace FGC.Core.Users.Events;

public sealed class SenhaAlteradaEvent : DomainEvent
{
    public Usuario Usuario { get; }

    public SenhaAlteradaEvent(Usuario usuario)
        => Usuario = usuario;
}