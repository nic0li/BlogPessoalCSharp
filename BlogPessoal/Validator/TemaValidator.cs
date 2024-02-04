﻿using BlogPessoal.Model;
using FluentValidation;

namespace BlogPessoal.Validator;

public class TemaValidator : AbstractValidator<Tema>
{
    public TemaValidator()
    {
        RuleFor(t => t.Descricao)
            .NotEmpty();
    }

}
