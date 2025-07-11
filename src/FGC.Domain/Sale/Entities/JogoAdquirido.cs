
using FGC.Core.Common.Entities;
using FGC.Core.Common.Entities.Interfaces;
using FGC.Core.Sale.Events;
using FGC.Core.Users.Entities;

namespace FGC.Core.Sale.Entities;

public class JogoAdquirido : EntityBase<Guid>, IAggregateRoot
{
    public Guid UsuarioId { get; private set; }
    public Guid JogoId { get; private set; }
    public decimal PrecoPago { get; private set; }
    public DateTime DataHora { get; private set; }
    public Usuario Usuario { get; private set; }
    protected JogoAdquirido() { }

    /// <summary>
    /// Cria um registro de compra de jogo na biblioteca do usuário.
    /// Gera JogoAdquiridoNaBibliotecaEvent.
    /// </summary>
    public JogoAdquirido(Guid usuarioId, Guid jogoId, decimal precoPago, Usuario usuario)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        JogoId = jogoId;
        PrecoPago = precoPago;
        DataHora = DateTime.UtcNow;
        Usuario = usuario;

        FGC.Core.Common.Events.DomainEvents.Raise(new JogoAdquiridoNaBibliotecaEvent(this));
    }
}