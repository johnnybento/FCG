using FGC.Core.Common.Repositories;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FGC.Infrastructure.Data;
using FGC.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;


namespace FGC.Infrastructure.Repositories.Users;

public class UsuarioRepository : EfRepository<Usuario, Guid>, IUserRepository
{
    public UsuarioRepository(ApplicationDBContext ctx)
    : base(ctx) { }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        var usuarios = await _dbSet.AsNoTracking().ToListAsync();
        return usuarios.FirstOrDefault(u => u.Email.Value == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var usuarios = await _dbSet.AsNoTracking().ToListAsync();
        return usuarios.Any(u => u.Email.Value == email);
    }

}
