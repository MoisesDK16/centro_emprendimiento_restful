using FluentValidation;
using Domain.Enums.Promocion;

namespace Application.Feautures.PromocionC.Commands
{
    public class CrearPromocionValidator : AbstractValidator<CrearPromocion>
    {
        public CrearPromocionValidator()
        {
            // Validar TipoPromocion: debe ser un valor válido del enum TipoPromocion
            RuleFor(x => x.TipoPromocion)
                .IsInEnum().WithMessage("El tipo de promoción no es válido.");

            // Validar FechaFin: debe ser posterior a FechaInicio
            RuleFor(x => x.FechaFin)
                .GreaterThan(x => x.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

            // Validar Descuento para el tipo de promoción DESCUENTO: debe ser mayor que 0 y menor que 100
            RuleFor(x => x.Descuento)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("El descuento debe ser mayor que 0.")
                .LessThan(100).WithMessage("El descuento debe ser menor que 100.")
                .When(x => x.TipoPromocion == TipoPromocion.DESCUENTO)
                .Unless(x => x.Descuento == null); // Permitimos que sea null si no es descuento

            // Validar CantidadCompra para el tipo de promoción REGALO: debe ser mayor que 0
            RuleFor(x => x.CantidadCompra)
                .GreaterThan(0).WithMessage("La cantidad de compra debe ser mayor que 0.")
                .When(x => x.TipoPromocion == TipoPromocion.REGALO)
                .Unless(x => x.CantidadCompra == null); // Permitimos que sea null si no es regalo

            // Validar CantidadGratis para el tipo de promoción REGALO: debe ser mayor que 0 y menor o igual a 100
            RuleFor(x => x.CantidadGratis)
                .GreaterThan(0).WithMessage("La cantidad gratis debe ser mayor que 0.")
                .LessThanOrEqualTo(100).WithMessage("La cantidad gratis no puede ser mayor que 100.")
                .When(x => x.TipoPromocion == TipoPromocion.REGALO)
                .Unless(x => x.CantidadGratis == null); // Permitimos que sea null si no es regalo

            // Validar que la lista de productos no esté vacía
            RuleFor(x => x.IdProductos)
                .NotEmpty().WithMessage("Debe seleccionar al menos un producto para la promoción.");

            // Validar NegocioId: debe ser un número positivo
            RuleFor(x => x.NegocioId)
                .GreaterThan(0).WithMessage("El negocio es obligatorio y debe ser válido.");
        }
    }
}
