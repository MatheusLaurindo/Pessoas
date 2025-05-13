using Pessoas.Server.Model;
using Pessoas.Server.Common;
using Pessoas.Server.DTOs.Request;
using Pessoas.Server.DTOs.Response;
using Pessoas.Server.Exceptions;
using Pessoas.Server.Extensoes;
using Pessoas.Server.Repositories.Interfaces;
using Pessoas.Server.Services.Interfaces;

namespace Pessoas.Server.Services
{
    public class PessoaService(IPessoaRepository repository) : IPessoaService
    {
        private readonly IPessoaRepository _repository = repository;

        public async Task<IEnumerable<GetPessoaResp>> GetAllAsync()
        {
            var pessoas = await _repository.GetAllAsync();

            return pessoas.Select(p => p.ToDto());
        }

        public async Task<GetPessoaRespPaginado> GetAllPaginatedAsync(int pagina, int linhasPorPagina)
        {
            var pessoas = await _repository.GetAllPaginatedAsync(pagina, linhasPorPagina);

            return pessoas;
        }

        public async Task<GetPessoaResp> GetByIdAsync(Guid id)
        {
            var pessoa = await _repository.GetByIdAsync(id);

            if (pessoa == null)
                return null;

            return pessoa.ToDto();
        }

        public async Task<Result<GetPessoaResp>> AddAsync(AdicionarPessoaRequest pessoa)
        {
            try
            {
                var cpf = pessoa.Cpf.Replace(".", "").Replace("-", "");

                var novaPessoa = Pessoa.Create(
                    pessoa.Nome,
                    pessoa.Email,
                    Convert.ToDateTime(pessoa.DataNascimento),
                    cpf,
                    pessoa.Sexo,
                    pessoa.Nacionalidade,
                    pessoa.Naturalidade,
                    pessoa.Endereco);

                if (!novaPessoa.FoiSucesso)
                    return Result<GetPessoaResp>.Falha(novaPessoa.Mensagem);

                var result = await _repository.AddAsync(novaPessoa.Valor);

                return Result<GetPessoaResp>.Sucesso(result.ToDto());
            }
            catch (DominioInvalidoException ex)
            {
                return Result<GetPessoaResp>.Falha(ex.Message);
            }
        }

        public async Task<Result<GetPessoaResp>> UpdateAsync(EditarPessoaRequest pessoa)
        {
            try
            {
                var pessoaExistente = await _repository.GetByIdAsync(pessoa.Id);

                if (pessoaExistente == null)
                    return Result<GetPessoaResp>.Falha("Pessoa não encontrada");

                pessoaExistente.SetNome(pessoa.Nome);
                pessoaExistente.SetEmail(pessoa.Email);
                pessoaExistente.SetDataNascimento(pessoa.DataNascimento);
                pessoaExistente.SetCpf(pessoa.Cpf);
                pessoaExistente.SetEndereco(pessoa.Endereco);
                pessoaExistente.SetSexo(pessoa.Sexo);
                pessoaExistente.SetNacionalidade(pessoa.Nacionalidade);
                pessoaExistente.SetNaturalidade(pessoa.Naturalidade);

                var result = await _repository.UpdateAsync(pessoaExistente);

                return Result<GetPessoaResp>.Sucesso(result.ToDto());
            }
            catch (DominioInvalidoException ex)
            {
                return Result<GetPessoaResp>.Falha(ex.Message);
            }
        }

        public async Task<Result<Guid>> DeleteAsync(Guid id)
        {
            try
            {
                var pessoaExistente = await _repository.GetByIdAsync(id);

                if (pessoaExistente == null)
                    return Result<Guid>.Falha("Pessoa não encontrada");

                await _repository.DeleteAsync(id);

                return Result<Guid>.Sucesso(id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Falha($"Erro ao remover dados: {ex.Message}");
            }
        }
    }
}
