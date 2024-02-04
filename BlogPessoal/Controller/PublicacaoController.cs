using BlogPessoal.Model;
using BlogPessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controller;

[Authorize]
[Route("~/publicacoes")]
[ApiController]
public class PublicacaoController : ControllerBase
{
    private readonly IPublicacaoService _publicacaoService;
    private readonly IValidator<Publicacao> _publicacaoValidator;

    public PublicacaoController(
        IPublicacaoService publicacaoService,
        IValidator<Publicacao> publicacaoValidator
        )
    {
        _publicacaoService = publicacaoService;
        _publicacaoValidator = publicacaoValidator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _publicacaoService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var Resposta = await _publicacaoService.GetById(id);

        if (Resposta is null)
        {
            return NotFound("Publicação não encontrada!");
        }
        return Ok(Resposta);
    }

    [HttpGet("titulo/{titulo}")]
    public async Task<ActionResult> GetByTitulo(string titulo)
    {
        return Ok(await _publicacaoService.GetByTitulo(titulo));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Publicacao publicacao)
    {
        var validarPublicacao = await _publicacaoValidator.ValidateAsync(publicacao);

        if (!validarPublicacao.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarPublicacao);

        var Resposta = await _publicacaoService.Create(publicacao);

        if (Resposta is null)
            return BadRequest("Publicação não encontrada!");

        return CreatedAtAction(nameof(GetById), new { id = Resposta.Id }, Resposta);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Publicacao publicacao)
    {
        if (publicacao.Id == 0)
            return BadRequest("Id da Publicação é inválido!");

        var validarPublicacao = await _publicacaoValidator.ValidateAsync(publicacao);

        if (!validarPublicacao.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarPublicacao);

        var Resposta = await _publicacaoService.Update(publicacao);

        if (Resposta is null)
            return NotFound("Publicação e/ou Tema não encontrados!");

        return Ok(Resposta);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var BuscaPublicacao = await _publicacaoService.GetById(id);

        if (BuscaPublicacao is null)
            return NotFound("Publicação não encontrada!");

        await _publicacaoService.Delete(BuscaPublicacao);

        return NoContent();
    }
}
