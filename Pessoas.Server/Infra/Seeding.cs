using Pessoas.Server.Enuns;
using Pessoas.Server.Model;

namespace Pessoas.Server.Infra
{
    public static class Seeding
    {
        public static void SeedPessoas(AppDbContext context)
        {
            if (!context.Pessoas.Any())
            {
                var pessoas = new List<Pessoa>();

                var tentativas = new[]
                {
                    Pessoa.Create("Ana Silva", "ana.silva@email.com", new DateTime(1990, 5, 12), "123.456.789-01", Sexo.Feminino, Nacionalidade.Brasileira, "São Paulo", "Rua das Flores, 100"),
                    Pessoa.Create("Carlos Pereira", "carlos.pereira@email.com", new DateTime(1985, 3, 20), "234.567.890-12", Sexo.Masculino, Nacionalidade.Brasileira, "Rio de Janeiro", "Av. Brasil, 234"),
                    Pessoa.Create("Mariana Costa", "mariana.costa@email.com", new DateTime(1992, 9, 1), "345.678.901-23", Sexo.Feminino, Nacionalidade.Brasileira, "Lisboa", "Rua A, 10"),
                    Pessoa.Create("João Mendes", "joao.mendes@email.com", new DateTime(1978, 7, 17), "456.789.012-34", Sexo.Masculino, Nacionalidade.Brasileira, "Belo Horizonte", "Rua B, 55"),
                    Pessoa.Create("Fernanda Oliveira", "fernanda.oliveira@email.com", new DateTime(2000, 1, 25), "567.890.123-45", Sexo.Feminino, Nacionalidade.Brasileira, "Curitiba", "Rua C, 98"),
                    Pessoa.Create("Lucas Lima", "lucas.lima@email.com", new DateTime(1995, 12, 5), "678.901.234-56", Sexo.Masculino, Nacionalidade.Brasileira, "Fortaleza", "Av. Central, 300"),
                    Pessoa.Create("Patrícia Rocha", "patricia.rocha@email.com", new DateTime(1988, 4, 9), "789.012.345-67", Sexo.Feminino, Nacionalidade.Brasileira, "Salvador", "Rua D, 88"),
                    Pessoa.Create("Eduardo Souza", "eduardo.souza@email.com", new DateTime(1993, 8, 30), "890.123.456-78", Sexo.Masculino, Nacionalidade.Brasileira, "Buenos Aires", "Rua E, 33"),
                    Pessoa.Create("Beatriz Martins", "beatriz.martins@email.com", new DateTime(1991, 2, 14), "901.234.567-89", Sexo.Feminino, Nacionalidade.Brasileira, "Recife", "Av. Norte, 102"),
                    Pessoa.Create("Gabriel Almeida", "gabriel.almeida@email.com", new DateTime(1983, 6, 11), "012.345.678-90", Sexo.Masculino, Nacionalidade.Brasileira, "Porto Alegre", "Rua F, 12"),
                    Pessoa.Create("Juliana Teixeira", "juliana.teixeira@email.com", new DateTime(1996, 11, 19), "112.233.445-56", Sexo.Feminino, Nacionalidade.Brasileira, "Madrid", "Rua G, 77"),
                    Pessoa.Create("Renato Barros", "renato.barros@email.com", new DateTime(1980, 10, 3), "223.344.556-67", Sexo.Masculino, Nacionalidade.Brasileira, "Florianópolis", "Av. Sul, 205"),
                    Pessoa.Create("Camila Duarte", "camila.duarte@email.com", new DateTime(1997, 7, 23), "334.455.667-78", Sexo.Feminino, Nacionalidade.Brasileira, "Natal", "Rua H, 45"),
                    Pessoa.Create("Thiago Nunes", "thiago.nunes@email.com", new DateTime(1994, 3, 8), "445.566.778-89", Sexo.Masculino, Nacionalidade.Brasileira, "Goiânia", "Rua I, 150"),
                    Pessoa.Create("Larissa Ferreira", "larissa.ferreira@email.com", new DateTime(1989, 9, 27), "556.677.889-90", Sexo.Feminino, Nacionalidade.Brasileira, "Campinas", "Rua J, 200"),

                };

                foreach (var resultado in tentativas)
                {
                    if (resultado.FoiSucesso)
                        pessoas.Add(resultado.Valor);
                }

                context.Pessoas.AddRange(pessoas);
                context.SaveChanges();
            }
        }

        public static void SeedUsuarios(AppDbContext context)
        {
            if (!context.Usuarios.Any())
            {
                var usuarios = new List<Usuario>();
                var permissoes = new List<UsuarioPermissao>();

                var tentativas = new[]
                {
                    Usuario.Create("admin@gmail.com", "admin123"),
                };

                foreach (var resultado in tentativas)
                {
                    if (!resultado.FoiSucesso || resultado.Valor == null)
                        continue;

                    var usuario = resultado.Valor;
                    usuarios.Add(usuario);

                    permissoes.AddRange(new[]
                    {
                        UsuarioPermissao.Create(usuario.Id, Permissao.Adicionar_Pessoa).Valor,
                        UsuarioPermissao.Create(usuario.Id, Permissao.Editar_Pessoa).Valor,
                        UsuarioPermissao.Create(usuario.Id, Permissao.Remover_Pessoa).Valor,
                        UsuarioPermissao.Create(usuario.Id, Permissao.Visualizar_Pessoa).Valor
                    });
                }

                context.Usuarios.AddRange(usuarios);
                context.UsuariosPermissoes.AddRange(permissoes);
                context.SaveChanges();
            }
        }

    }
}
