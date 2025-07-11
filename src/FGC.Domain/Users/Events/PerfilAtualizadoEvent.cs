
using FGC.Core.Common.Events;
using FGC.Core.Users.Entities;

namespace FGC.Core.Users.Events;

public sealed class PerfilAtualizadoEvent : DomainEvent
{
    public Usuario Usuario { get; }

    public PerfilAtualizadoEvent(Usuario usuario)
        => Usuario = usuario;
}