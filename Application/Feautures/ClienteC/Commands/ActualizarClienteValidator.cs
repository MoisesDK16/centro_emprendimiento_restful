using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.ClienteC.Commands
{
    public class ActualizarClienteValidator : AbstractValidator<ActualizarCliente>
    {
        public ActualizarClienteValidator()
        {
            RuleFor(x => x.Nombres)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.");

            RuleFor(x => x.PrimerApellido)
                .NotEmpty().WithMessage("El primer apellido es obligatorio.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El correo electrónico no es válido.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Ciudad)
                .NotEmpty().WithMessage("La ciudad es obligatoria.");
        }
    }
}
