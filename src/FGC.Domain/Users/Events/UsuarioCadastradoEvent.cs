using FGC.Core.Common.Events;
using FGC.Core.Users.Entities;

namespace FGC.Core.Users.Events;

public sealed class UsuarioCadastradoEvent : DomainEvent
{
    
    public Usuario Usuario { get; }

    public UsuarioCadastradoEvent(Usuario usuario)
        => Usuario = usuario;    
}