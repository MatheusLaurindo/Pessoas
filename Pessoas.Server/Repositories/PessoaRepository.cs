using Microsoft.EntityFrameworkCore;
using Pessoas.Server.Common;
using Pessoas.Server.DTOs.Response;
using Pessoas.Server.Exceptions;
using Pessoas.Server.Infra;
using Pessoas.Server.Model;
using Pessoas.Server.Repositories.Interfaces;

namespace Pessoas.Server.Repositories
{
    public class PessoaRepository(AppDbContext contexto) : IPessoaRepository
    {
        private readonly AppDbContext _contexto = contexto;

        public async Task<IEnumerable<Pessoa>> GetAllAsync()
            => await _contexto.Pessoas
            .AsNoTracking()
            .ToListAsync();

        public async Task<GetPessoaRespPaginado> GetAllPaginatedAsync(int pagina, int linhasPorPagina)
        {
            var query = _contexto.Pessoas
                .AsNoTracking();

            var count = await query.CountAsync();

            var pessoas = await query
                .OrderByDescending(x => x.DataCadastro)
                .Skip(pagina * linhasPorPagina)
                .Take(linhasPorPagina)
                .ToListAsync();

            var pessoasResponse = pessoas.Select(p => new GetPessoaResp
            {
                Id = p.Id,
                Nome = p.Nome,
                Email = p.Email,
                DataNascimento = p.DataNascimento.ToString("dd/MM/yyyy"),
                Cpf = p.Cpf,
                Sexo = p.Sexo?.ToString() == "0" ? "" : p.Sexo.ToString(),
                Nacionalidade = p.Nacionalidade?.ToString() == "0" ? "" : p.Nacionalidade.ToString(),
                Naturalidade = p.Naturalidade,
                Endereco = p.Endereco
            });

            return new GetPessoaRespPaginado
            {
                Total = count,
                Data = pessoasResponse
            };
        }

        public async Task<Pessoa> GetByIdAsync(Guid id)
            => await _contexto.Pessoas
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Pessoa> AddAsync(Pessoa pessoa)
        {
            var cpfExiste = await _contexto.Pessoas
                .AsNoTracking()
                .AnyAsync(x => x.Cpf == pessoa.Cpf);

            if (cpfExiste)
                throw new DominioInvalidoException("Este CPF já está cadastrado para outra pessoa.");

            _contexto.Pessoas.Add(pessoa);

            await _contexto.SaveChangesAsync();

            return pessoa;
        }

        public async Task<Pessoa> UpdateAsync(Pessoa pessoa)
        {
            var pessoaCpf = await _contexto.Pessoas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Cpf == pessoa.Cpf);

            if (pessoaCpf != null && pessoaCpf.Id != pessoa.Id)
                throw new ArgumentException("Este CPF já está cadastrado para outra pessoa.");

            pessoa.SetDataAtualizacao();

            _contexto.Pessoas.Update(pessoa);

            await _contexto.SaveChangesAsync();

            return pessoa;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var pessoa = await _contexto.Pessoas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pessoa == null)
                return false;

            _contexto.Pessoas.Remove(pessoa);

            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}
