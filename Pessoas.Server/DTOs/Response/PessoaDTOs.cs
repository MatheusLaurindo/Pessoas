namespace Pessoas.Server.DTOs.Response
{
    public sealed record GetPessoaResp
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public string Email { get; init; }
        public DateTime DataNascimento { get; init; }
        public string Cpf { get; init; }
        public string Sexo { get; init; }
        public string Nacionalidade { get; init; }
        public string Naturalidade { get; init; }
        public string Endereco { get; init; }
    }
}
