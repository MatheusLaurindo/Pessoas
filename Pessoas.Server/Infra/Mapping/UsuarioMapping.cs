using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pessoas.Server.Model;

namespace Pessoas.Server.Infra.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Senha)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .HasMany(x => x.VinculosPermissao)
                .WithOne(x => x.Usuario)
                .HasForeignKey(x => x.UsuarioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
