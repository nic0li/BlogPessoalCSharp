using BlogPessoal.Model;
using FluentValidation;

namespace BlogPessoal.Validator;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(u => u.Nome)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(u => u.NomeDeUsuario)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Senha)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(u => u.Foto)
            .MaximumLength(5000);

    }

}