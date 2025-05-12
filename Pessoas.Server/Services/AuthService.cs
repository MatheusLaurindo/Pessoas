using Microsoft.EntityFrameworkCore;
using Pessoas.Server.Common;
using Pessoas.Server.Infra;
using Pessoas.Server.Services.Interfaces;
using Pessoas.Server.Utils;

namespace Pessoas.Server.Services
{
    public class AuthService(AppDbContext contexto, IConfiguration configuration) : IAuthService
    {
        private readonly AppDbContext _contexto = contexto;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Result<JwtToken>> Authenticate(string email, string senha)
        {
            try
            {
                var usuario = await _contexto.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

                if (usuario == null)
                    return Result<JwtToken>.Falha("Usuário ou senha inválidos");

                var permissoes = await _contexto.UsuariosPermissoes
                    .Where(up => up.UsuarioId == usuario.Id)
                    .Select(up => (int)up.Permissao)
                    .ToArrayAsync();

                var jwtTokenResult = JwtToken.Criar(
                    issuer: _configuration["JWT:ISSUER"],
                    audience: _configuration["JWT:AUDIENCE"],
                    secretKey: _configuration["JWT:KEY"],
                    id: usuario.Id,
                    email: usuario.Email,
                    permissoes: permissoes);

                if (!jwtTokenResult.FoiSucesso)
                    return Result<JwtToken>.Falha(jwtTokenResult.Mensagem);

                var jwtToken = jwtTokenResult.Valor;

                return Result<JwtToken>.Sucesso(jwtToken);
            }
            catch (Exception ex)
            {
                return Result<JwtToken>.Falha(ex.Message);
            }
        }
    }
}
