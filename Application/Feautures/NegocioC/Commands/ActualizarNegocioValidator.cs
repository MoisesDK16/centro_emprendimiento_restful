using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.NegocioC.Commands
{
   public class ActualizarNegocioValidator : AbstractValidator<ActualizarNegocio>
    {
        public ActualizarNegocioValidator()
        {
            RuleFor(p => p.Nombre)
                .MaximumLength(50).WithMessage("{PropertyName} no debe exceder de 50 caracteres.");
            RuleFor(p => p.Direccion)
                .MaximumLength(100).WithMessage("{PropertyName} no debe exceder de 100 caracteres.");
            RuleFor(p => p.Telefono)
                .MaximumLength(15).WithMessage("{PropertyName} no debe exceder de 15 caracteres.");
            RuleFor(p => p.Descripcion)
                .MaximumLength(1024).WithMessage("{PropertyName} no debe exceder de 1024 caracteres.");
            RuleFor(p => p.Categorias)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");
        }
    }
}
