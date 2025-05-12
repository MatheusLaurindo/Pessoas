using Pessoas.Server.Repositories.Interfaces;
using Pessoas.Server.Repositories;
using Pessoas.Server.Services.Interfaces;
using Pessoas.Server.Services;

namespace Pessoas.Server.Infra.Installers
{
    public class DependencyInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddHttpContextAccessor();
        }
    }
}
