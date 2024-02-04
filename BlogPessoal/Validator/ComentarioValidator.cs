using BlogPessoal.Model;
using FluentValidation;

namespace BlogPessoal.Validator;

public class ComentarioValidator : AbstractValidator<Comentario>
{
    public ComentarioValidator()
    {
        RuleFor(p => p.Texto)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }

}

