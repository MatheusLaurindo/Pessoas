using System.ComponentModel.DataAnnotations;

namespace Pessoas.Server.DTOs.Request
{
    public sealed record LoginRequest
    {
        /// <summary>
        /// Email do usuário. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; init; } = null;

        /// <summary>
        /// Senha do usuário. <b>Obrigatório</b>.
        /// </summary>
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; init; } = null;
    }
}
