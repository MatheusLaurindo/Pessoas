using Pessoas.Server.DTOs.Response;
using Pessoas.Server.Model;
using System.Globalization;

namespace Pessoas.Server.Extensoes
{
    public static class PessoaExtensions
    {
        public static GetPessoaResp ToDto(this Pessoa p) =>
            new()
            {
                Id = p.Id,
                Nome = p.Nome,
                Email = p.Email,
                DataNascimento = p.DataNascimento.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Cpf = p.Cpf,
                Sexo = p.Sexo.ToString(),
                Nacionalidade = p.Nacionalidade.ToString(),
                Naturalidade = p.Naturalidade,
                Endereco = p.Endereco
            };
    }
}
