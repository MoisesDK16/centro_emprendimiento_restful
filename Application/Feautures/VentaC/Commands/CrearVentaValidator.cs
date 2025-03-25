using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.VentaC.Commands
{
    public class CrearVentaValidator : AbstractValidator<CrearVenta>
    {
        public CrearVentaValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor a 0.");

            RuleFor(x => x.NegocioId)
                .GreaterThan(0).WithMessage("El ID del negocio debe ser mayor a 0.");

            RuleFor(x => x.Detalles)
                .NotEmpty().WithMessage("Debe registrar al menos un detalle para la venta.");

            RuleForEach(x => x.Detalles).ChildRules(detalle =>
            {
                detalle.RuleFor(d => d.ProductoId)
                    .GreaterThan(0).WithMessage("El ID del producto debe ser mayor a 0.");

                detalle.RuleFor(d => d.StockId)
                    .GreaterThan(0).WithMessage("El ID del stock debe ser mayor a 0.");

                detalle.RuleFor(d => d.Cantidad)
                    .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

                detalle.RuleFor(d => d.Precio)
                    .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

                detalle.RuleFor(d => d.Total)
                    .GreaterThan(0).WithMessage("El total debe ser mayor a 0.")
                    .Must((detalle, total) => total == detalle.Precio * detalle.Cantidad)
                    .WithMessage("El total debe ser igual al precio multiplicado por la cantidad.");

                detalle.RuleFor(d => d.PromocionId)
                    .GreaterThanOrEqualTo(0).WithMessage("El ID de promoción no puede ser negativo.");
            });
        }
    }
}
