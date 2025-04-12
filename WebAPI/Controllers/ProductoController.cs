using Application.Feautures.ProductoC.Commands;
using Application.Feautures.ProductoC.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Feautures.ProductoC.Commands.CrearProducto;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class ProductoController : BaseApiController
    {
        public ProductoController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Emprendedor")]
        [HttpPost("crear")]
        public async Task<IActionResult> CrearProducto([FromForm] CrearProductoParameters comando)
        {
            return Ok(await Mediator.Send(
                new CrearProducto
                {
                    Codigo = comando.Codigo,
                    Nombre = comando.Nombre,
                    Descripcion = comando.Descripcion,
                    Estado = comando.Estado,
                    Iva = comando.Iva,

                    PrecioCompra = comando.PrecioCompra,
                    PrecioVenta = comando.PrecioVenta,
                    Cantidad = comando.Cantidad,

                    FechaElaboracion = comando.FechaElaboracion,
                    FechaCaducidad = comando.FechaCaducidad,
                    FechaIngreso = comando.FechaIngreso,

                    CategoriaId = comando.CategoriaId,
                    NegocioId = comando.NegocioId,

                    UserId = User.FindFirst("uid")?.Value,
                    Imagen = comando.Imagen
                }));
        }

        [Authorize(Roles = "Emprendedor")]
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarProducto([FromForm] ActualizarProducto comando)
        {
            return Ok(await Mediator.Send(
                new ActualizarProducto
                {
                    Id = comando.Id,
                    Codigo = comando.Codigo,
                    Nombre = comando.Nombre,
                    Descripcion = comando.Descripcion,
                    Iva = comando.Iva,
                    PrecioCompra = comando.PrecioCompra,
                    PrecioVenta = comando.PrecioVenta,
                    Cantidad = comando.Cantidad,
                    FechaElaboracion = comando.FechaElaboracion,
                    FechaCaducidad = comando.FechaCaducidad,
                    FechaIngreso = comando.FechaIngreso,
                    CategoriaId = comando.CategoriaId,
                    StockId = comando.StockId,
                    UserId = User.FindFirst("uid")?.Value,
                    Imagen = comando.Imagen
                }));
        }

        [Authorize(Roles = "Emprendedor,Vendedor")]
        [HttpGet("listarProductos")]
         public async Task<IActionResult> ListarProductos([FromQuery] ListarProductosParameters filter)
         {
            var productos = await Mediator.Send(new ListarProductos
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                NegocioId = filter.NegocioId,
                CategoriaId = filter.CategoriaId,
                NombreProducto = filter.NombreProducto,
                UserId = User.FindFirst("uid")?.Value,
            });

            return Ok(productos);
         }
        [Authorize(Roles = "Emprendedor,Vendedor")]
        [HttpGet("productoById")]
        public async Task<IActionResult> ProductoById([FromQuery] ProductoByIdParameters request)
        {
            var producto = await Mediator.Send(new ProductoById
            {
                ProductoId = request.ProductoId,
                NegocioId = request.NegocioId,
                UserId = User.FindFirst("uid")?.Value
            });
            return Ok(producto);
        }

        [Authorize(Roles = "Emprendedor,Vendedor")]
        [HttpGet("selectProductos")]
        public async Task<IActionResult> SelectProductos([FromQuery] SelectProductosParameters selectProductos)
        {
            var productos = await Mediator.Send(new SelectProductos
            {
                NegocioId = selectProductos.NegocioId,
                UserId = User.FindFirst("uid")?.Value
            });
            return Ok(productos);
        }
    }
}
