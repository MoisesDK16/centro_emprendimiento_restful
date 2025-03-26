using FluentValidation;

namespace Application.Feautures.CategoriaC.Commands
{
    public class CrearCategoriaValidator : AbstractValidator<CrearCategoriaComando>
    {
        public CrearCategoriaValidator() {

            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} es requerido")
                .NotNull()
                .MaximumLength(30).WithMessage("{PropertyName} no debe exceder los 30 caracteres");

            RuleFor(p => p.Tipo)
                .NotEmpty().WithMessage("{PropertyName} es requerido")
                .NotNull();

            RuleFor(p => p.Descripcion)
                .MaximumLength(200).WithMessage("{PropertyName} no debe exceder los 200 caracteres");

        }

    }
}
