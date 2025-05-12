using Pessoas.Server.Model;

namespace Pessoas.Server.Repositories.Interfaces
{
    public interface IPessoaRepository
    {
        Task<IEnumerable<Pessoa>> GetAllAsync();
        Task<IEnumerable<Pessoa>> GetAllPaginatedAsync(int pagina, int linhasPorPagina);
        Task<Pessoa> GetByIdAsync(Guid id);
        Task<Pessoa> AddAsync(Pessoa pessoa);
        Task<Pessoa> UpdateAsync(Pessoa pessoa);
        Task<bool> DeleteAsync(Guid id);
    }
}
