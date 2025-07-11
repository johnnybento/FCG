
using FGC.Core.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FGC.Infrastructure.Data.Configurations.Catalog;

public class PromocaoConfiguration : IEntityTypeConfiguration<Promocao>
{
    public void Configure(EntityTypeBuilder<Promocao> builder)
    {
        builder.ToTable("Promocoes");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.JogoId)
            .IsRequired();

        builder.Property(p => p.Desconto)
            .IsRequired()
            .HasColumnType("decimal(5,4)");

        builder.Property(p => p.Inicio)
            .IsRequired();

        builder.Property(p => p.Termino)
            .IsRequired();
        
        builder.HasOne<Jogo>()
               .WithMany()
               .HasForeignKey(p => p.JogoId);
    }
}
