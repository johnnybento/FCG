using FCG.Core.Users.Enum;
using FGC.Core.Common.Entities;
using FGC.Core.Common.Entities.Interfaces;
using FGC.Core.Exceptions;
using FGC.Core.Sale.Entities;
using FGC.Core.Sale.Events;
using FGC.Core.Users.Events;
using FGC.Core.Users.ValueObjects;

namespace FGC.Core.Users.Entities;

public class Usuario : EntityBase<Guid>, IAggregateRoot
{    
    public string Nome { get; private set; }
    public EmailVo Email { get; private set; }
    public string SenhaHash { get; private set; }
    public UserRole Papel { get; private set; }

    private readonly List<JogoAdquirido> _biblioteca = new();

    public IReadOnlyCollection<JogoAdquirido> Biblioteca
        => _biblioteca.AsReadOnly();
    protected Usuario() { }

    public Usuario(string nome, EmailVo email, string senhaHash, UserRole papel)
    {

        if (string.IsNullOrEmpty(nome) || nome.Replace(" ", "").Equals(""))
            throw new DomainException("Nome não pode ser vazio ou nulo.");

        Id = Guid.NewGuid();
        Nome = nome ?? throw new DomainException("Nome não pode ser vazio ou nulo");
        Email = email ?? throw new DomainException("Email não pode ser vazio ou nulo");
        SenhaHash = senhaHash ?? throw new DomainException("Hash da senha inválido.");
        Papel = papel;

        Common.Events.DomainEvents.Raise(new UsuarioCadastradoEvent(this));
    }

    public void AtualizarPerfil(string novoNome, EmailVo novoEmail)
    {
        if(novoNome.Replace(" ","").Equals("") || string.IsNullOrEmpty(novoNome))
            throw new DomainException("Nome não pode ser vazio ou nulo.");

        Nome = novoNome;
        Email = novoEmail ?? throw new DomainException("Email não pode ser vazio ou nulo.");



       Common.Events.DomainEvents.Raise(new PerfilAtualizadoEvent(this));
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaHash))
            throw new DomainException("Hash de senha inválido.");

        SenhaHash = novaSenhaHash;
        Common.Events.DomainEvents.Raise(new SenhaAlteradaEvent(this));
    }

    public void Desativar()
    {
        Common.Events.DomainEvents.Raise(new UsuarioDesativadoEvent(this));
    }

    public void DefinirPapel(UserRole novoPapel)
    {
        Papel = novoPapel;
    }
    public void AdquirirJogo(JogoAdquirido compra)
    {
        if (_biblioteca.Exists(x => x.JogoId == compra.JogoId))
            throw new DomainException("Jogo já adquirido.");

        _biblioteca.Add(compra);
        Common.Events.DomainEvents.Raise(new JogoAdquiridoNaBibliotecaEvent(compra));
    }

}