using BlogPessoal.Model;
using BlogPessoal.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controller;

[Authorize]
[Route("~/[Controller]")]
[ApiController]
public class PublicacaoController : ControllerBase
{
    private readonly IPublicacaoService _service;
    private readonly IValidator<Publicacao> _validator;

    public PublicacaoController(IPublicacaoService service, IValidator<Publicacao> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var resposta = await _service.GetById(id);

        if (resposta is null)
        {
            return NotFound("Publicação não encontrada!");
        }
        return Ok(resposta);
    }

    [HttpGet("Titulo/{titulo}")]
    public async Task<ActionResult> GetByTitulo(string titulo)
    {
        return Ok(await _service.GetByTitulo(titulo));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Publicacao publicacao)
    {
        var validarPublicacao = await _validator.ValidateAsync(publicacao);

        if (!validarPublicacao.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarPublicacao);

        var resposta = await _service.Create(publicacao);

        if (resposta is null)
            return BadRequest("Publicação não encontrada!");

        return CreatedAtAction(nameof(GetById), new { id = resposta.Id }, resposta);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Publicacao publicacao)
    {
        if (publicacao.Id == 0)
            return BadRequest("Id da Publicação é inválido!");

        var validarPublicacao = await _validator.ValidateAsync(publicacao);

        if (!validarPublicacao.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarPublicacao);

        var resposta = await _service.Update(publicacao);

        if (resposta is null)
            return NotFound("Publicação e/ou Tema não encontrados!");

        return Ok(resposta);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var publicacaoBuscada = await _service.GetById(id);

        if (publicacaoBuscada is null)
            return NotFound("Publicação não encontrada!");

        await _service.Delete(publicacaoBuscada);

        return NoContent();
    }
}
