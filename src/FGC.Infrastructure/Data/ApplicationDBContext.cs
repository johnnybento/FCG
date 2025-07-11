using FGC.Core.Catalog.Entities;
using FGC.Core.Sale.Entities;
using FGC.Core.Users.Entities;
using FGC.Core.Common.Events.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FGC.Infrastructure.Data;

public class ApplicationDBContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Jogo> Jogos { get; set; } = null!;
    public DbSet<Promocao> Promocoes { get; set; } = null!;
    public DbSet<JogoAdquirido> JogoAdquiridos { get; set; } = null!;

    private readonly string _connectionString;

    private readonly IDomainEventDispatcher _dispatcher;

    public ApplicationDBContext() { }

    public ApplicationDBContext(IDomainEventDispatcher dispatcher) 
    {
        IConfiguration configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        _connectionString = configuration.GetConnectionString("ConnectionString");

        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher), "Domain event dispatcher cannot be null.");
    } 


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await _dispatcher.DispatchAsync();
        return result;
    }
    public ApplicationDBContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
    }

}
