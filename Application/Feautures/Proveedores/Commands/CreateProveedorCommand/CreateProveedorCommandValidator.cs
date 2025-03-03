using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.Proveedores.Commands.CreateProveedorCommand
{
    public class CreateProveedorCommandValidator: AbstractValidator<CreateProveedorCommand>
    {
        public CreateProveedorCommandValidator()
        {
            RuleFor(p => p.nombre)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
            RuleFor(p => p.telefono)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(15).WithMessage("{PropertyName} must not exceed 15 characters.");
            RuleFor(p => p.correo)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
            RuleFor(p => p.direccion)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.ruc)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(13).WithMessage("{PropertyName} must not exceed 13 characters.");
        }
    }
}
