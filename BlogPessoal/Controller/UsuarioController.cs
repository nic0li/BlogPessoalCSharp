using BlogPessoal.Model;
using BlogPessoal.Security;
using BlogPessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controller;

[Route("~/usuarios")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IValidator<Usuario> _usuarioValidator;
    private readonly IAuthService _authService;

    public UsuarioController(
        IUsuarioService usuarioService,
        IValidator<Usuario> usuarioValidator,
        IAuthService authService
        )
    {
        _usuarioService = usuarioService;
        _usuarioValidator = usuarioValidator;
        _authService = authService;
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _usuarioService.GetAll());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var Resposta = await _usuarioService.GetById(id);

        if (Resposta is null)
        {
            return NotFound("Usuário não encontrado!");
        }
        return Ok(Resposta);
    }

    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<ActionResult> Create([FromBody] Usuario usuario)
    {
        var validarUsuario = await _usuarioValidator.ValidateAsync(usuario);

        if (!validarUsuario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarUsuario);

        var Resposta = await _usuarioService.Create(usuario);

        if (Resposta is null)
            return BadRequest("Usuário já cadastrado!");

        return CreatedAtAction(nameof(GetById), new { id = Resposta.Id }, Resposta);
    }

    [Authorize]
    [HttpPut("atualizar")]
    public async Task<ActionResult> Update([FromBody] Usuario usuario)
    {
        if (usuario.Id == 0)
            return BadRequest("Id do Usuário é inválido!");

        var validarUsuario = await _usuarioValidator.ValidateAsync(usuario);

        if (!validarUsuario.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, validarUsuario);

        var UsuarioUpdate = await _usuarioService.GetByUsuario(usuario.NomeDeUsuario);

        if ((UsuarioUpdate is not null) && (UsuarioUpdate.Id != usuario.Id))
            return BadRequest("O Usuário (e-mail) já está em uso por outro usuário.");

        var Resposta = await _usuarioService.Update(usuario);

        if (Resposta is null)
            return BadRequest("Usuário não encontrado!");

        return Ok(Resposta);
    }

    [AllowAnonymous]
    [HttpPost("logar")]
    public async Task<IActionResult> Autenticar([FromBody] Login usuarioLogin)
    {
        var Resposta = await _authService.Autenticar(usuarioLogin);

        if (Resposta == null)
        {
            return Unauthorized("Usuário e/ou Senha inválidos!");
        }

        return Ok(Resposta);
    }
}
