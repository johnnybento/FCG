
using FGC.Core.Common.Events;
using FGC.Core.Sale.Entities;

namespace FGC.Core.Sale.Events;

/// <summary>
/// Disparado quando um usuário adquire um jogo na biblioteca.
/// </summary>
public sealed class JogoAdquiridoNaBibliotecaEvent : DomainEvent
{
    public JogoAdquirido Compra { get; }

    public JogoAdquiridoNaBibliotecaEvent(JogoAdquirido compra)
        => Compra = compra;
}