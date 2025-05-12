using Microsoft.IdentityModel.Tokens;
using Pessoas.Server.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pessoas.Server.Utils
{
    public sealed class JwtToken
    {
        public string JWT_TOKEN { get; set; }

        private JwtToken(string jwtToken)
        {
            JWT_TOKEN = jwtToken;
        }

        public static Result<JwtToken> Criar(
            string issuer,
            string audience,
            string secretKey,
            Guid id,
            string email,
            int[] permissoes)
        {
            if (string.IsNullOrWhiteSpace(issuer))
                return Result<JwtToken>.Falha("Issuer não pode ser nulo ou vazio");

            if (string.IsNullOrWhiteSpace(audience))
                return Result<JwtToken>.Falha("Audience não pode ser nulo ou vazio");

            if (string.IsNullOrWhiteSpace(secretKey))
                return Result<JwtToken>.Falha("SecretKey não pode ser nulo ou vazio");

            if (id == Guid.Empty)
                return Result<JwtToken>.Falha("Id inválido");

            if (string.IsNullOrWhiteSpace(email))
                return Result<JwtToken>.Falha("Email não pode ser nulo ou vazio");

            if (permissoes == null || permissoes.Length == 0)
                return Result<JwtToken>.Falha("Permissões não podem ser nulas ou vazias");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.PrimarySid, id.ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            claims.AddRange(permissoes
                 .Select(permissao => new Claim(ClaimTypes.Role, permissao.ToString())));

            var token = GerarJwtToken(secretKey, claims);

            return Result<JwtToken>.Sucesso(new JwtToken(token));
        }

        private static string GerarJwtToken(string key, IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class JwtSettings
        {
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string SecretKey { get; set; }
        }
    }
}
