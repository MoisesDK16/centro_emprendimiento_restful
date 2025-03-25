using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace Application.Feautures.StockC.Commands
{
    public class ActualizarStockValidator : AbstractValidator<ActualizarStock>
    {
        public ActualizarStockValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del stock debe ser mayor a 0.");

            RuleFor(x => x.PrecioCompra)
                .GreaterThan(0).WithMessage("El precio de compra debe ser mayor a 0.");

            RuleFor(x => x.PrecioVenta)
                .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a 0.")
                .GreaterThanOrEqualTo(x => x.PrecioCompra)
                .WithMessage("El precio de venta no puede ser menor que el precio de compra.");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

            RuleFor(x => x.FechaElaboracion)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("La fecha de elaboración no puede ser futura.");

            RuleFor(x => x.FechaCaducidad)
                .GreaterThan(x => x.FechaElaboracion)
                .WithMessage("La fecha de caducidad debe ser posterior a la fecha de elaboración.");

            RuleFor(x => x.FechaIngreso)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("La fecha de ingreso no puede ser futura.");
        }
    }
}
