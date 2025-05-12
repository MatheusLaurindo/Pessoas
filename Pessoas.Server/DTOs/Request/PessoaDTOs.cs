using Pessoas.Server.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Pessoas.Server.DTOs.Request
{
    public sealed record AdicionarPessoaRequest
    {
        /// <summary>
        /// Nome da pessoa. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Nome' é obrigatório")]
        public string Nome { get; init; } = null;

        /// <summary>
        /// Data de Nascimento. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Data de Nascimento' é obrigatório")]
        public DateTime DataNascimento { get; init; }

        /// <summary>
        /// CPF. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'CPF' é obrigatório")]
        public string Cpf { get; init; } = null;

        public string Email { get; init; } = null;
        public string Endereco { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }

    public sealed record AdicionarPessoaRequestV2
    {
        /// <summary>
        /// Nome da Pessoa. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Nome' é obrigatório")]
        public string Nome { get; init; } = null;

        /// <summary>
        /// Data de Nascimento. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Data de Nascimento' é obrigatório")]
        public DateTime DataNascimento { get; init; }

        /// <summary>
        /// CPF. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'CPF' é obrigatório")]
        public string Cpf { get; init; } = null;

        /// <summary>
        /// Endereço. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Endereço' é obrigatório")]
        public string Endereco { get; init; } = null;

        public string Email { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }

    public sealed record EditarPessoaRequest
    {
        /// <summary>
        /// Id. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Id' é obrigatório")]
        public Guid Id { get; init; } = Guid.Empty;

        /// <summary>
        /// Nome da pessoa. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Nome' é obrigatório")]
        public string Nome { get; init; } = null;

        /// <summary>
        /// Data de Nascimento. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Data de Nascimento' é obrigatório")]
        public DateTime DataNascimento { get; init; }

        /// <summary>
        /// CPF. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'CPF' é obrigatório")]
        public string Cpf { get; init; } = null;

        public string Email { get; init; } = null;
        public string Endereco { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }

    public sealed record EditarPessoaRequestV2
    {
        /// <summary>
        /// Id. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Id' é obrigatório")]
        public Guid Id { get; init; } = Guid.Empty;

        /// <summary>
        /// Nome da Pessoa. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Nome' é obrigatório")]
        public string Nome { get; init; } = null;

        /// <summary>
        /// Data de Nascimento. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Data de Nascimento' é obrigatório")]
        public DateTime DataNascimento { get; init; }

        /// <summary>
        /// CPF. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'CPF' é obrigatório")]
        public string Cpf { get; init; } = null;

        /// <summary>
        /// Endereço. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "O parametro 'Endereço' é obrigatório")]
        public string Endereco { get; init; } = null;

        public string Email { get; init; } = null;
        public Sexo? Sexo { get; init; } = null;
        public Nacionalidade? Nacionalidade { get; init; } = null;
        public string Naturalidade { get; init; } = null;
    }
}
