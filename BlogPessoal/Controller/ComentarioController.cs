using BlogPessoal.Model;
using BlogPessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controller;

[Authorize]
[Route("~/comentarios")]
[ApiController]
public class ComentarioController : ControllerBase
{
    private readonly IComentarioService _comentarioService;
    private readonly IValidator<Comentario> _comentarioValidator;

    public ComentarioController(
        IComentarioService comentarioService,
        IValidator<Comentario> comentarioValidator
        )
    {
        _comentarioService = comentarioService;
        _comentarioValidator = comentarioValidator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _comentarioService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var Resposta = await _comentarioService.GetById(id);

        if (Resposta is null)
        {
            return NotFound("Comentário não encontrada!");
        }
        return Ok(Resposta);
    }

    [HttpGet("texto/{texto}")]
    public async Task<ActionResult> GetByTexto(string texto)
    {
        return Ok(await _comentarioService.GetByTexto(texto));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Comentario comentario)
    {
        var validarComentario = await _comentarioValidator.ValidateAsync(comentario);

        if (!validarComentario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarComentario);

        var Resposta = await _comentarioService.Create(comentario);

        if (Resposta is null)
            return BadRequest("Comentário não encontrado!");

        return CreatedAtAction(nameof(GetById), new { id = Resposta.Id }, Resposta);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Comentario comentario)
    {
        if (comentario.Id == 0)
            return BadRequest("Id da Comentario é inválido!");

        var validarComentario = await _comentarioValidator.ValidateAsync(comentario);

        if (!validarComentario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarComentario);

        var Resposta = await _comentarioService.Update(comentario);

        if (Resposta is null)
            return NotFound("Comentário e/ou Publicação não encontrados!");

        return Ok(Resposta);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var BuscaComentario = await _comentarioService.GetById(id);

        if (BuscaComentario is null)
            return NotFound("Comentário não encontrado!");

        await _comentarioService.Delete(BuscaComentario);

        return NoContent();
    }
}
