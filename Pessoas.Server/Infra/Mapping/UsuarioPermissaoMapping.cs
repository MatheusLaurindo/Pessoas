using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pessoas.Server.Model;

namespace Pessoas.Server.Infra.Mapping
{
    public class UsuarioPermissaoMapping : IEntityTypeConfiguration<UsuarioPermissao>
    {
        public void Configure(EntityTypeBuilder<UsuarioPermissao> builder)
        {
            builder.HasKey(x => new { x.UsuarioId, x.Permissao });

            builder.Property(x => x.Permissao)
                .IsRequired();

            builder
                .HasOne(x => x.Usuario)
                .WithMany(x => x.VinculosPermissao)
                .HasForeignKey(x => x.UsuarioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
