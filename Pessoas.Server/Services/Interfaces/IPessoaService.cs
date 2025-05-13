using Pessoas.Server.Common;
using Pessoas.Server.DTOs.Request;
using Pessoas.Server.DTOs.Response;
using Pessoas.Server.Model;

namespace Pessoas.Server.Services.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<GetPessoaResp>> GetAllAsync();
        Task<GetPessoaRespPaginado> GetAllPaginatedAsync(int pagina, int linhasPorPagina);
        Task<GetPessoaResp> GetByIdAsync(Guid id);
        Task<Result<GetPessoaResp>> AddAsync(AdicionarPessoaRequest pessoa);
        Task<Result<GetPessoaResp>> UpdateAsync(EditarPessoaRequest pessoa);
        Task<Result<Guid>> DeleteAsync(Guid id);
    }
}
