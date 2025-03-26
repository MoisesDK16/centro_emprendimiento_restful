using Domain.Enums.Promocion;
using FluentValidation;

namespace Application.Feautures.PromocionC.Commands
{
    public class ActualizarPromocionValidator : AbstractValidator<ActualizarPromocion>
    {
        public ActualizarPromocionValidator() {

            // Validar FechaFin: debe ser posterior a FechaInicio
            RuleFor(x => x.FechaFin)
                .GreaterThan(x => x.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

            // Validar que la lista de productos no esté vacía
            RuleFor(x => x.IdProductos)
                .NotEmpty().WithMessage("Debe seleccionar al menos un producto para la promoción.");

        }

    }
}
