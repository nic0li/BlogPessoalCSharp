using BlogPessoal.Model;
using BlogPessoal.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controller;

[Authorize]
[Route("~/[Controller]")]
[ApiController]
public class TemaController : ControllerBase
{
    private readonly ITemaService _service;
    private readonly IValidator<Tema> _validator;

    public TemaController(ITemaService service,IValidator<Tema> validator)
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
            return NotFound("Tema não encontrado!");
        }
        return Ok(resposta);
    }

    [HttpGet("Descricao/{descricao}")]
    public async Task<ActionResult> GetByDescricao(string descricao)
    {
        return Ok(await _service.GetByDescricao(descricao));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Tema tema)
    {
        var validarTema = await _validator.ValidateAsync(tema);

        if (!validarTema.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarTema);

        var resposta = await _service.Create(tema);

        if (resposta is null)
            return BadRequest("Tema não encontrado!");

        return CreatedAtAction(nameof(GetById), new { id = resposta.Id }, resposta);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Tema tema)
    {
        if (tema.Id == 0)
            return BadRequest("Id do Tema é inválido!");

        var validarTema = await _validator.ValidateAsync(tema);

        if (!validarTema.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarTema);

        var resposta = await _service.Update(tema);

        if (resposta is null)
            return NotFound("Tema não encontrado!");

        return Ok(resposta);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var temaBuscado = await _service.GetById(id);

        if (temaBuscado is null)
            return NotFound("Tema não encontrado!");

        await _service.Delete(temaBuscado);

        return NoContent();
    }
}
