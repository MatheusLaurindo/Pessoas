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
public class PessoaController : PessoaBaseController
{
    private readonly IPessoaService _service;
    private readonly ILogger<PessoaBaseController> _logger;

    public PessoaController(IPessoaService service, ILogger<PessoaBaseController> logger) : base(service, logger)
    {
        _service = service;
        _logger = logger;
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
}

/// <summary>  
/// Gerencia operações relacionadas a pessoas. (V2)
/// </summary>  
[Authorize]
[ApiController]
[Route("api/v2/pessoa")]
[ApiExplorerSettings(GroupName = "v2")]
[Produces(MediaTypeNames.Application.Json)]
public class PessoaControllerV2 : PessoaBaseController
{
    private readonly IPessoaService _service;
    private readonly ILogger<PessoaBaseController> _logger;

    public PessoaControllerV2(IPessoaService service, ILogger<PessoaBaseController> logger) : base(service, logger)
    {
        _service = service;
        _logger = logger;
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
}
