using Pessoas.Server.Common;
using Pessoas.Server.Utils;

namespace Pessoas.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<JwtToken>> Authenticate(string email, string senha);
    }
}
