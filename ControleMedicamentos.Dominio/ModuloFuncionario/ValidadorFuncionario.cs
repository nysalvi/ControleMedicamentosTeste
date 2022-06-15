﻿using System;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class ValidadorFuncionario : AbstractValidator<Funcionario>
    {
        public ValidadorFuncionario()
        {
            RuleFor(x => x.Nome)
                 .NotNull().WithMessage("Campo 'Nome' não pode ser nulo")
                 .NotEmpty().WithMessage("Campo 'Nome' não pode ser vazio");

            RuleFor(x => x.Login)
                .NotNull().WithMessage("Campo 'Login' não pode ser nulo")
                .NotEmpty().WithMessage("Campo 'Login' não pode ser vazio");

            RuleFor(x => x.Senha)
                .NotNull().WithMessage("Campo 'Senha' não pode ser nulo")
                .NotEmpty().WithMessage("Campo 'Senha' não pode ser vazio");
        }
    }
}
