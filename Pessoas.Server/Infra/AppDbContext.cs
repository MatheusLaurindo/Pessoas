using Microsoft.EntityFrameworkCore;
using Pessoas.Server.Infra.Mapping;
using Pessoas.Server.Model;

namespace Pessoas.Server.Infra
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioPermissao> UsuariosPermissoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new UsuarioPermissaoMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
