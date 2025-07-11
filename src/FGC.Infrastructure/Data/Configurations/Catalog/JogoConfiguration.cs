using FGC.Core.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FGC.Infrastructure.Data.Configurations.Catalog;
public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
{
    public void Configure(EntityTypeBuilder<Jogo> builder)
    {
        builder.ToTable("Jogos");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(j => j.Descricao)
            .HasMaxLength(1000);

        builder.Property(j => j.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}

