using BlogPessoal.Model;
using BlogPessoal.Security;
using BlogPessoal.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controller;

[Route("~/[Controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _service;
    private readonly IValidator<Usuario> _validator;
    private readonly IAuthService _authService;

    public UsuarioController(IUsuarioService service, IValidator<Usuario> validator, IAuthService authService)
    {
        _service = service;
        _validator = validator;
        _authService = authService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var resposta = await _service.GetById(id);

        if (resposta is null)
        {
            return NotFound("Usuário não encontrado!");
        }
        return Ok(resposta);
    }

    [AllowAnonymous]
    [HttpPost("Cadastro")]
    public async Task<ActionResult> Create([FromBody] Usuario usuario)
    {
        var validarUsuario = await _validator.ValidateAsync(usuario);

        if (!validarUsuario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarUsuario);

        var resposta = await _service.Create(usuario);

        if (resposta is null)
            return BadRequest("Usuário já cadastrado!");

        return CreatedAtAction(nameof(GetById), new { id = resposta.Id }, resposta);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Autenticar([FromBody] UsuarioLogin usuarioLogin)
    {
        var resposta = await _authService.Autenticar(usuarioLogin);

        if (resposta == null)
        {
            return Unauthorized("Usuário e/ou Senha inválidos!");
        }

        return Ok(resposta);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Usuario usuario)
    {
        if (usuario.Id == 0)
            return BadRequest("Id do Usuário é inválido!");

        var validarUsuario = await _validator.ValidateAsync(usuario);

        if (!validarUsuario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarUsuario);

        var usuarioBuscado = await _service.GetByUsuario(usuario.NomeDeUsuario);

        if ((usuarioBuscado is not null) && (usuarioBuscado.Id != usuario.Id))
            return BadRequest("O Usuário (e-mail) já está em uso por outro usuário.");

        var resposta = await _service.Update(usuario);

        if (resposta is null)
            return BadRequest("Usuário não encontrado!");

        return Ok(resposta);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuarioBuscado = await _service.GetById(id);

        if (usuarioBuscado is null)
            return NotFound("Usuário não encontrado!");

        await _service.Delete(usuarioBuscado);

        return NoContent();
    }
}
