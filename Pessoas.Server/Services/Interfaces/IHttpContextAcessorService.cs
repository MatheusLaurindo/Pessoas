namespace Pessoas.Server.Services.Interfaces
{
    public interface IHttpContextAcessorService
    {
        Guid GetId();
        GetInfoUsuario GetPermissoes();
        bool IsAuthenticated();
    }

    public class GetInfoUsuario
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}
