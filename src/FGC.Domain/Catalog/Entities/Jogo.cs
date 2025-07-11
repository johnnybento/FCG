using FGC.Core.Catalog.Events;
using FGC.Core.Common.Entities;
using FGC.Core.Common.Entities.Interfaces;
using FGC.Core.Exceptions;
using FGC.Core.Sale.Entities;


namespace FGC.Core.Catalog.Entities;

public class Jogo : EntityBase<Guid>, IAggregateRoot
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }

    private readonly List<JogoAdquirido> _biblioteca = new();

    private Jogo() { }

    public Jogo(string titulo, string descricao, decimal preco)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título do jogo não pode ser vazio.");

        if (preco < 0)
            throw new DomainException("Preço não pode ser negativo.");


        Id = Guid.NewGuid();
        Titulo = titulo.Trim();
        Descricao = descricao?.Trim();
        Preco = preco;



        Common.Events.DomainEvents.Raise(new JogoCadastradoEvent(this));
    }

    public void AtualizarDados(string novoTitulo, string novaDescricao, decimal novoPreco)
    {
        if (string.IsNullOrWhiteSpace(novoTitulo))
            throw new DomainException("Título é obrigatório. Título" + nameof(novoTitulo));
        if (novoPreco < 0)
            throw new DomainException("Preço não pode ser negativo");

        Titulo = novoTitulo;
        Descricao = novaDescricao;
        Preco = novoPreco;

    }

}
