using Application.Features.Proveedores.Commands.UpdateProveedorCommand;
using FluentValidation;

namespace Application.Feautures.Proveedores.Commands.UpdateProveedorComand
{
    public class UpdateProveedorCommandValidator: AbstractValidator<UpdateProveedorCommand>
    {
        public UpdateProveedorCommandValidator()
        {

            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

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
