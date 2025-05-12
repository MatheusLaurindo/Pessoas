using Microsoft.AspNetCore.Mvc;
using Pessoas.Server.Atributos;
using Pessoas.Server.Common;
using Pessoas.Server.DTOs.Request;
using Pessoas.Server.DTOs.Response;
using Pessoas.Server.Enuns;
using Pessoas.Server.Model;
using Pessoas.Server.Services.Interfaces;
using System.Net.Mime;

namespace Pessoas.Server.Controllers
{
    public abstract class PessoaBaseController(IPessoaService service, ILogger<PessoaBaseController> logger) : ControllerBase
    {
        private readonly IPessoaService _service = service;
        private readonly ILogger<PessoaBaseController> _logger = logger;

        /// <summary>
        /// Retorna todas as pessoas.
        /// </summary>
        [AppAuthorize(Permissao.Visualizar_Pessoa)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var pessoas = await _service.GetAllAsync();

            return Ok(APITypedResponse<IEnumerable<GetPessoaResp>>.Create(pessoas, true, ""));
        }

        /// <summary>
        /// Retorna pessoas de forma paginada.
        /// </summary>
        /// <param name="pagina">Número da página.</param>
        /// <param name="linhasPorPagina">Quantidade de registros por página.</param>
        [AppAuthorize(Permissao.Visualizar_Pessoa)]
        [HttpGet("paginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaginatedAsync(int pagina = 1, int linhasPorPagina = 10)
        {
            var pessoas = await _service.GetAllPaginatedAsync(pagina, linhasPorPagina);

            return Ok(APITypedResponse<IEnumerable<GetPessoaResp>>.Create(pessoas, true, ""));
        }

        /// <summary>
        /// Retorna uma pessoa pelo ID.
        /// </summary>
        /// <param name="id">ID da pessoa.</param>
        [AppAuthorize(Permissao.Visualizar_Pessoa)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var pessoa = await _service.GetByIdAsync(id);

            if (pessoa == null)
                return NotFound(APITypedResponse<GetPessoaResp>.Create(pessoa, false, "Pessoa não encontrada."));

            return Ok(APITypedResponse<GetPessoaResp>.Create(pessoa, true, ""));
        }

        /// <summary>
        /// Remove uma pessoa pelo ID.
        /// </summary>
        /// <param name="id">ID da pessoa a ser removida.</param>
        [AppAuthorize(Permissao.Remover_Pessoa)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var pessoa = await _service.GetByIdAsync(id);

            if (pessoa == null)
            {
                _logger.LogWarning("Pessoa não encontrada: {@Id}", id);

                return NotFound(APITypedResponse<Guid>.Create(id, false, "Pessoa não encontrada."));
            }

            var result = await _service.DeleteAsync(id);

            if (!result.FoiSucesso)
            {
                ModelState.AddModelError("", result.Mensagem);

                _logger.LogWarning("Erro ao remover pessoa: {@Mensagem}", result.Mensagem);

                return BadRequest(APITypedResponse<Guid>.Create(id, false, result.Mensagem));
            }

            _logger.LogInformation("Pessoa removida com sucesso: {@Id}", id);

            return Ok(APITypedResponse<Guid>.Create(id, true, result.Mensagem));
        }
    }
}
