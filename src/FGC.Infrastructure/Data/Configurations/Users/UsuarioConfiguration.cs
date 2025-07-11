using FGC.Core.Sale.Entities;
using FGC.Core.Users.Entities;
using FGC.Core.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FGC.Infrastructure.Data.Configurations.Users;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
             .HasConversion(
           vo => vo.Value,
           valor => EmailVo.Create(valor)

       )
            .IsRequired()
            .HasMaxLength(200)
            .HasConversion(
                vo => vo.Value,
                valor => EmailVo.Create(valor)
            );

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Papel)
            .IsRequired()
            .HasConversion<int>();


        builder.HasMany(typeof(JogoAdquirido))
               .WithOne()
               .HasForeignKey("UsuarioId")
               .OnDelete(DeleteBehavior.Cascade);


        builder.Metadata.FindNavigation(nameof(Usuario.Biblioteca))
               .SetPropertyAccessMode(PropertyAccessMode.Field);

    }
}
