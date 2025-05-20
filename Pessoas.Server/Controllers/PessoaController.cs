using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pessoas.Server.Atributos;
using Pessoas.Server.Common;
using Pessoas.Server.DTOs.Request;
using Pessoas.Server.DTOs.Response;
using Pessoas.Server.Enuns;
using Pessoas.Server.Model;
using Pessoas.Server.Services.Interfaces;
using System.Net.Mime;

namespace Pessoas.Server.Controllers;

/// <summary>  
/// Gerencia operações relacionadas a pessoas.  
/// </summary>  
[Authorize]
[ApiController]
[Route("api/v1/pessoa")]
[ApiExplorerSettings(GroupName = "v1")]
[Produces(MediaTypeNames.Application.Json)]
public class PessoaV1Controller : ControllerBase
{
    private readonly IPessoaService _service;
    private readonly ILogger<PessoaV1Controller> _logger;

    public PessoaV1Controller(IPessoaService service, ILogger<PessoaV1Controller> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todas as pessoas.
    /// </summary>
    [AppAuthorize(Permissao.Visualizar_Pessoa)]
    [HttpGet]
    [ProducesResponseType(typeof(APITypedResponse<IEnumerable<GetPessoaResp>>), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(APIPaginatedResponse<IEnumerable<GetPessoaResp>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedAsync(int pagina = 1, int linhasPorPagina = 10)
    {
        var pessoas = await _service.GetAllPaginatedAsync(pagina, linhasPorPagina);

        return Ok(APIPaginatedResponse<IEnumerable<GetPessoaResp>>.Create(pessoas.Data, pessoas.Total));
    }

    /// <summary>
    /// Retorna uma pessoa pelo ID.
    /// </summary>
    /// <param name="id">ID da pessoa.</param>
    [AppAuthorize(Permissao.Visualizar_Pessoa)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(APITypedResponse<GetPessoaResp>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var pessoa = await _service.GetByIdAsync(id);

        if (pessoa == null)
            return NotFound(APITypedResponse<GetPessoaResp>.Create(pessoa, false, "Pessoa não encontrada."));

        return Ok(APITypedResponse<GetPessoaResp>.Create(pessoa, true, ""));
    }

    /// <summary>
    /// Adiciona uma nova pessoa.
    /// </summary>
    /// <param name="request">Dados da nova pessoa.</param>
    [AppAuthorize(Permissao.Adicionar_Pessoa)]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(APITypedResponse<GetPessoaResp>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAsync([FromBody] AdicionarPessoaRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido: {@ModelState}", ModelState);

            return BadRequest(ModelState);
        }

        var result = await _service.AddAsync(request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);

            _logger.LogWarning("Erro ao adicionar pessoa: {@Mensagem}", result.Mensagem);

            return BadRequest(APITypedResponse<GetPessoaResp>.Create(null, false, result.Mensagem));
        }

        _logger.LogInformation("Pessoa adicionada com sucesso: {@Pessoa}", result.Valor);

        return Ok(APITypedResponse<GetPessoaResp>.Create(result.Valor, true, result.Mensagem));
    }

    /// <summary>
    /// Atualiza uma pessoa existente.
    /// </summary>
    /// <param name="request">Dados atualizados da pessoa.</param>
    [AppAuthorize(Permissao.Editar_Pessoa)]
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(APITypedResponse<GetPessoaResp>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromBody] EditarPessoaRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido: {@ModelState}", ModelState);

            return BadRequest(ModelState);
        }

        var pessoa = await _service.GetByIdAsync(request.Id);

        if (pessoa == null)
        {
            _logger.LogWarning("Pessoa não encontrada: {@Id}", request.Id);

            return NotFound(APITypedResponse<GetPessoaResp>.Create(null, false, "Pessoa não encontrada."));
        }

        var result = await _service.UpdateAsync(request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);

            _logger.LogWarning("Erro ao atualizar pessoa: {@Mensagem}", result.Mensagem);

            return BadRequest(APITypedResponse<GetPessoaResp>.Create(null, false, result.Mensagem));
        }

        _logger.LogInformation("Pessoa atualizada com sucesso: {@Pessoa}", result.Valor);

        return Ok(APITypedResponse<GetPessoaResp>.Create(result.Valor, true, result.Mensagem));
    }

    /// <summary>
    /// Remove uma pessoa pelo ID.
    /// </summary>
    /// <param name="id">ID da pessoa a ser removida.</param>
    [AppAuthorize(Permissao.Remover_Pessoa)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(APITypedResponse<Guid>), StatusCodes.Status200OK)]
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

/// <summary>  
/// Gerencia operações relacionadas a pessoas. (V2)
/// </summary>  
[Authorize]
[ApiController]
[Route("api/v2/pessoa")]
[ApiExplorerSettings(GroupName = "v2")]
[Produces(MediaTypeNames.Application.Json)]
public class PessoaV2Controller : ControllerBase
{
    private readonly IPessoaService _service;
    private readonly ILogger<PessoaV2Controller> _logger;

    public PessoaV2Controller(IPessoaService service, ILogger<PessoaV2Controller> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todas as pessoas.
    /// </summary>
    [AppAuthorize(Permissao.Visualizar_Pessoa)]
    [HttpGet]
    [ProducesResponseType(typeof(APITypedResponse<IEnumerable<GetPessoaResp>>), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(APIPaginatedResponse<IEnumerable<GetPessoaResp>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedAsync(int pagina = 1, int linhasPorPagina = 10)
    {
        var pessoas = await _service.GetAllPaginatedAsync(pagina, linhasPorPagina);

        return Ok(APIPaginatedResponse<IEnumerable<GetPessoaResp>>.Create(pessoas.Data, pessoas.Total));
    }

    /// <summary>
    /// Retorna uma pessoa pelo ID.
    /// </summary>
    /// <param name="id">ID da pessoa.</param>
    [AppAuthorize(Permissao.Visualizar_Pessoa)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(APITypedResponse<GetPessoaResp>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var pessoa = await _service.GetByIdAsync(id);

        if (pessoa == null)
            return NotFound(APITypedResponse<GetPessoaResp>.Create(pessoa, false, "Pessoa não encontrada."));

        return Ok(APITypedResponse<GetPessoaResp>.Create(pessoa, true, ""));
    }

    /// <summary>
    /// Adiciona uma nova pessoa. (V2)
    /// </summary>
    /// <param name="request">Dados da nova pessoa. O campo <c>Endereço</c> é obrigatório.</param>
    [AppAuthorize(Permissao.Adicionar_Pessoa)]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(APITypedResponse<GetPessoaResp>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAsync([FromBody] AdicionarPessoaRequestV2 request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido: {@ModelState}", ModelState);

            return BadRequest(ModelState);
        }

        var v1Request = new AdicionarPessoaRequest
        {
            Nome = request.Nome,
            DataNascimento = request.DataNascimento,
            Cpf = request.Cpf,
            Email = request.Email,
            Endereco = request.Endereco,
            Sexo = request.Sexo,
            Nacionalidade = request.Nacionalidade,
            Naturalidade = request.Naturalidade
        };

        var result = await _service.AddAsync(v1Request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);

            _logger.LogWarning("Erro ao adicionar pessoa: {@Mensagem}", result.Mensagem);

            return BadRequest(APITypedResponse<GetPessoaResp>.Create(null, false, result.Mensagem));
        }

        _logger.LogInformation("Pessoa adicionada com sucesso: {@Pessoa}", result.Valor);

        return Ok(APITypedResponse<GetPessoaResp>.Create(result.Valor, true, result.Mensagem));
    }

    /// <summary>
    /// Atualiza uma pessoa existente. (V2)
    /// </summary>
    /// <param name="request">Dados atualizados da pessoa. O campo <c>Endereço</c> é obrigatório.</param>
    [AppAuthorize(Permissao.Editar_Pessoa)]
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(APITypedResponse<GetPessoaResp>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromBody] EditarPessoaRequestV2 request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido: {@ModelState}", ModelState);

            return BadRequest(ModelState);
        }

        var pessoa = await _service.GetByIdAsync(request.Id);

        if (pessoa == null)
        {
            _logger.LogWarning("Pessoa não encontrada: {@Id}", request.Id);

            return NotFound(APITypedResponse<GetPessoaResp>.Create(null, false, "Pessoa não encontrada."));
        }

        var v1Request = new EditarPessoaRequest
        {
            Nome = request.Nome,
            DataNascimento = request.DataNascimento,
            Cpf = request.Cpf,
            Email = request.Email,
            Endereco = request.Endereco,
            Sexo = request.Sexo,
            Nacionalidade = request.Nacionalidade,
            Naturalidade = request.Naturalidade
        };

        var result = await _service.UpdateAsync(v1Request);

        if (!result.FoiSucesso)
        {
            ModelState.AddModelError("", result.Mensagem);

            _logger.LogWarning("Erro ao atualizar pessoa: {@Mensagem}", result.Mensagem);

            return BadRequest(APITypedResponse<GetPessoaResp>.Create(null, false, result.Mensagem));
        }

        _logger.LogInformation("Pessoa atualizada com sucesso: {@Pessoa}", result.Valor);

        return Ok(APITypedResponse<GetPessoaResp>.Create(result.Valor, true, result.Mensagem));
    }

    /// <summary>
    /// Remove uma pessoa pelo ID.
    /// </summary>
    /// <param name="id">ID da pessoa a ser removida.</param>
    [AppAuthorize(Permissao.Remover_Pessoa)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(APITypedResponse<Guid>), StatusCodes.Status200OK)]
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
