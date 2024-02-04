using BlogPessoal.Model;
using FluentValidation;

namespace BlogPessoal.Validator;

public class PublicacaoValidator : AbstractValidator<Publicacao>
{
    public PublicacaoValidator()
    {
        RuleFor(p => p.Titulo)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(100);

        RuleFor(p => p.Texto)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }

}

