using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pessoas.Server.Model;

namespace Pessoas.Server.Infra.Mapping
{
    public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.DataNascimento);

            builder.HasIndex(p => p.Cpf)
                .IsUnique();

            builder.Property(p => p.Cpf)
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(255);

            builder.Property(p => p.Sexo);

            builder.Property(p => p.Nacionalidade)
                .HasMaxLength(50);

            builder.Property(p => p.Naturalidade);
        }
    }
}
