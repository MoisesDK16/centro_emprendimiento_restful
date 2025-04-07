using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.UsuarioC.Commands
{
    public class ActualizarUsuarioValidator : AbstractValidator<ActualizarUsuario>
    {
        public ActualizarUsuarioValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(20).WithMessage("El nombre de usuario no puede exceder los 20 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.");

            RuleFor(x => x.CiudadOrigen)
                .NotEmpty().WithMessage("La ciudad de origen es obligatoria.")
                .MaximumLength(50).WithMessage("La ciudad de origen no puede exceder los 50 caracteres.");
        }
    }
}
