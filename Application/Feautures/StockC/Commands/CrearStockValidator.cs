using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace Application.Feautures.StockC.Commands
{
    public class CrearStockValidator : AbstractValidator<CrearStock>
    {
        public CrearStockValidator()
        {
            RuleFor(x => x.ProductoId)
                .GreaterThan(0).WithMessage("El ID del producto debe ser mayor a 0.");

            RuleFor(x => x.PrecioCompra)
                .GreaterThan(0).WithMessage("El precio de compra debe ser mayor a 0.");

            RuleFor(x => x.PrecioVenta)
                .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0.")
                .GreaterThanOrEqualTo(x => x.PrecioCompra).WithMessage("El precio de venta no puede ser menor al precio de compra.");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

            RuleFor(x => x.FechaCaducidad)
                .GreaterThan(x => x.FechaElaboracion).WithMessage("La fecha de caducidad debe ser posterior a la fecha de elaboración.");

            /*RuleFor(x => x.FechaIngreso)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de ingreso no puede ser futura.");*/
        }
    }
}
