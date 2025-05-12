using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pessoas.Server.Common;
using Pessoas.Server.DTOs.Request;
using Pessoas.Server.Services.Interfaces;
using Pessoas.Server.Utils;

namespace Pessoas.Server.Controllers
{
    /// <summary>
    /// Endpoints relacionados à autenticação e permissões de usuários.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        /// <summary>
        /// Realiza o login do usuário e insere um token JWT no HTTP Cookies
        /// </summary>
        /// <param name="request">Credenciais de login do usuário.</param>
        /// <returns>Status da autenticação.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="400">Credenciais inválidas ou erro de validação.</response>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("LoginRequest inválido: {ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            var result = await _authService.Authenticate(request.Email, request.Senha);

            if (!result.FoiSucesso)
            {
                _logger.LogError("Falha ao autenticar usuário: {Mensagem}", result.Mensagem);

                return BadRequest(APITypedResponse<JwtToken>.Create(null, false, result.Mensagem));
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddHours(8),
                Secure = true,                 
                SameSite = SameSiteMode.None  
            };

            HttpContext.Response.Cookies.Append("jwt_token", result.Valor.JWT_TOKEN, cookieOptions);

            _logger.LogInformation("Usuário autenticado com sucesso: {Email}", request.Email);

            return Ok(APITypedResponse<JwtToken>.Create(null, true, result.Mensagem));
        }
    }
}
