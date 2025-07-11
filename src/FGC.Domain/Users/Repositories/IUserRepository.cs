using FGC.Core.Common.Repositories.Interfaces;
using FGC.Core.Users.Entities;

namespace FGC.Core.Users.Repositories;

/// <summary>
/// Operações específicas de consulta/criação de Usuario
/// </summary>
public interface IUserRepository : IRepository<Usuario, Guid>
{
    /// <summary>Encontra um usuário pelo e-mail (ou retorna null).</summary>
    Task<Usuario> GetByEmailAsync(string email);

    /// <summary>Verifica se já existe um usuário com este e-mail.</summary>
    Task<bool> EmailExistsAsync(string email);
}