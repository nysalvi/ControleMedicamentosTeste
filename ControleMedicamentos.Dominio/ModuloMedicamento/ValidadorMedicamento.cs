using System;
using FluentValidation;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class ValidadorMedicamento : AbstractValidator<Medicamento>
    {
        ValidadorFornecedor val;
        public ValidadorMedicamento()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("Campo 'Nome' não pode ser nulo")
                .NotEmpty().WithMessage("Campo 'Nome' não pode ser vazio");

            RuleFor(x => x.Descricao)
                .NotNull().WithMessage("Campo 'Descricao' não pode ser nulo")
                .NotEmpty().WithMessage("Campo 'Descricao' não pode ser vazio");

            RuleFor(x => x.Lote)
                .NotNull().WithMessage("Campo 'Lote' não pode ser nulo")
                .NotEmpty().WithMessage("Campo 'Lote' não pode ser vazio");

            RuleFor(x => x.Validade)
                .NotNull().WithMessage("Campo 'Validade' não pode ser nulo")
                .NotEmpty().WithMessage("Campo 'Validade' não pode ser vazio");

            RuleFor(x => x.Fornecedor)
                //.Cascade(val.Validate())
                .NotNull().WithMessage("Campo 'Fornecedor' não pode ser nulo");
                            

            RuleFor(x => x.QuantidadeDisponivel)            
                .GreaterThanOrEqualTo(0).WithMessage
                    ("Campo 'QuantidadeDisponivel' não pode ser negativo");
        }
    }
}
