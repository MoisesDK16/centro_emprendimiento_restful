using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.NegocioC.Commands
{
    public class CrearNegocioValidator : AbstractValidator<CrearNegocio>
    {
        public CrearNegocioValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} no debe exceder de 50 caracteres.");
            RuleFor(p => p.Direccion)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} no debe exceder de 100 caracteres.");
            RuleFor(p => p.Telefono)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .NotNull()
                .MaximumLength(15).WithMessage("{PropertyName} no debe exceder de 15 caracteres.");
            RuleFor(p => p.Descripcion)
                .MaximumLength(1024).WithMessage("{PropertyName} no debe exceder de 1024 caracteres.");
            RuleFor(p => p.EmprendedorId)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");
            RuleFor(p => p.Categoria)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");

        }
    }
}
