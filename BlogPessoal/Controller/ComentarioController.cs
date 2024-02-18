using BlogPessoal.Model;
using BlogPessoal.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controller;

[Authorize]
[Route("~/[Controller]")]
[ApiController]
public class ComentarioController : ControllerBase
{
    private readonly IComentarioService _service;
    private readonly IValidator<Comentario> _validator;

    public ComentarioController(IComentarioService service,IValidator<Comentario> validator)
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
            return NotFound("Comentário não encontrada!");
        }
        return Ok(resposta);
    }

    [HttpGet("Texto/{texto}")]
    public async Task<ActionResult> GetByTexto(string texto)
    {
        return Ok(await _service.GetByTexto(texto));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Comentario comentario)
    {
        var validarComentario = await _validator.ValidateAsync(comentario);

        if (!validarComentario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarComentario);

        var resposta = await _service.Create(comentario);

        if (resposta is null)
            return BadRequest("Comentário não encontrado!");

        return CreatedAtAction(nameof(GetById), new { id = resposta.Id }, resposta);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Comentario comentario)
    {
        if (comentario.Id == 0)
            return BadRequest("Id da Comentario é inválido!");

        var validarComentario = await _validator.ValidateAsync(comentario);

        if (!validarComentario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarComentario);

        var resposta = await _service.Update(comentario);

        if (resposta is null)
            return NotFound("Comentário e/ou Publicação não encontrados!");

        return Ok(resposta);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var comentarioBuscado = await _service.GetById(id);

        if (comentarioBuscado is null)
            return NotFound("Comentário não encontrado!");

        await _service.Delete(comentarioBuscado);

        return NoContent();
    }
}
