using FGC.Core.Sale.Entities;
using FGC.Core.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FGC.Infrastructure.Data.Configurations.Sales;

public class JogoAdquiridoConfiguration : IEntityTypeConfiguration<JogoAdquirido>
{
    public void Configure(EntityTypeBuilder<JogoAdquirido> builder)
    {
        builder.ToTable("JogoAdquiridos");
        builder.HasKey(ja => ja.Id);

        builder.Property(ja => ja.UsuarioId)
            .IsRequired();

        builder.Property(ja => ja.JogoId)
            .IsRequired();

        builder.Property(ja => ja.PrecoPago)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ja => ja.DataHora)
            .IsRequired();

        builder.HasOne<Usuario>()
               .WithMany(u => u.Biblioteca)
               .HasForeignKey(ja => ja.UsuarioId);
               
    }
}