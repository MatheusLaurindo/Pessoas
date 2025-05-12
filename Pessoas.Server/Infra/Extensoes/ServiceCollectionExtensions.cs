using Pessoas.Server.Infra.Installers;
using System.Reflection;

namespace Pessoas.Server.Infra.Extensoes
{
    public static class ServiceCollectionExtensions
    {
        public static void InstallServicesFromAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
