using Microsoft.AspNetCore.Authorization;
using Pessoas.Server.Enuns;
using System.Data;

namespace Pessoas.Server.Atributos
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AppAuthorize : AuthorizeAttribute
    {
        private static string GetRoles<T>(T[] roles)
        {
            string permissoes = string.Join(",", roles.Select(x => Convert.ToInt32(x)));

            if (!string.IsNullOrWhiteSpace(permissoes))
                permissoes = $",{permissoes}";

            return permissoes;
        }

        public AppAuthorize(params Permissao[] roles) : base()
        {
            Roles = GetRoles(roles);
        }
    }
}
