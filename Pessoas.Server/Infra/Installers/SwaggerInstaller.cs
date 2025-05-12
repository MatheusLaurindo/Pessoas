using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Pessoas.Server.Infra.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pessoas.API V1", Version = "v1", Description = "API para gerenciamento de pessoas e autenticação JWT." });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "Pessoas.API V2", Version = "v2", Description = "Gerenciamento de cadastro e pessoas." });

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
