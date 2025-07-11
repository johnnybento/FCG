namespace FGC.Application.Common.Ports;

public interface IPasswordHasher
{
    /// <summary>
    /// Gera um hash seguro da senha limpa.
    /// </summary>
    string Hash(string senha);

    /// <summary>
    /// Verifica se a senha limpa corresponde ao hash armazenado.
    /// </summary>
    bool Verify(string senha, string hash);
}
