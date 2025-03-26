using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.ProductoC.Commands
{
    public class ActualizarProductoValidator : AbstractValidator<ActualizarProducto>
    {
        public ActualizarProductoValidator()
        {
            RuleFor(x => x.Codigo)
                .Matches(@"^[A-Za-z0-9]+$").WithMessage("El código del producto debe ser alfanumérico.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción del producto es obligatoria.");

            RuleFor(x => x.Imagen)
                .Must(img => img == null || (img.Length > 0 && img.Length <= 5)) // max 5MB
                .WithMessage("La imagen no debe superar los 5 MB.");

            RuleFor(x => x.CategoriaId)
                .GreaterThan(0).WithMessage("La categoría es obligatoria y debe ser válida.");

            RuleFor(x => x.PrecioCompra)
                .GreaterThan(0).WithMessage("El precio de compra debe ser mayor que 0.");

            RuleFor(x => x.PrecioVenta)
                .GreaterThan(0).WithMessage("El precio de venta debe ser mayor que 0.");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor que 0.");

            RuleFor(x => x.FechaCaducidad)
                .GreaterThan(x => x.FechaElaboracion).WithMessage("La fecha de caducidad debe ser posterior a la fecha de elaboración.");

        }
    }
}
