using FGC.Core.Common.Events;
using FGC.Core.Users.Entities;

namespace FGC.Core.Users.Events;

public sealed class UsuarioDesativadoEvent : DomainEvent
{
    public Usuario Usuario { get; }

    public UsuarioDesativadoEvent(Usuario usuario)
        => Usuario = usuario;
}
