using Application.DTOs.Detalles;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Domain.Entities;
using Domain.Enums.Promocion;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Application.Services.StockS
{
    public class StockService
    {
        private readonly IReadOnlyRepositoryAsync<Stock> _stockRepository;
        private readonly IReadOnlyRepositoryAsync<Producto> _productoRepository;

        public StockService(IReadOnlyRepositoryAsync<Stock> stockRepository, IReadOnlyRepositoryAsync<Producto> productRepository)
        {
            _stockRepository = stockRepository;
            _productoRepository = productRepository;
        }

       public async Task RestarStock(int cantidad, long idProducto, long stockId)
       {
            var stock = await _stockRepository.FirstOrDefaultAsync(new StockSpecification(idProducto, stockId)) 
                ?? throw new ApiException("No se encontró el stock del producto en el proceso de substracción de stock");
            stock.Cantidad -= cantidad;
            await _stockRepository.UpdateAsync(stock);
       }

        public async Task VerificarPrecioAsync(long idProducto, long stockId, decimal detallePrecio)
        {
            var stock = await _stockRepository.FirstOrDefaultAsync(new StockSpecification(idProducto, stockId))
                ?? throw new ApiException($"No se encontró el stock del producto {idProducto}");

            if (stock.PrecioVenta != detallePrecio)
            {
                throw new ApiException("El precio del producto no coincide con el precio receptado desde backend");
            }
        }

        public async Task<decimal> AplicarIva(long idProducto, long NegocioId, decimal detallePrecio)
        {
            var productoFound = await _productoRepository.FirstOrDefaultAsync(new ProductoSpecification(idProducto, NegocioId))
                ?? throw new ApiException($"No se encontró el producto con id: {idProducto}");

            if (productoFound.Iva != 0)
            {
                decimal iva = (productoFound.Iva / 100) * detallePrecio;
                detallePrecio += iva;
            }

            return detallePrecio;
        }

        public async Task VerificarPrecioConPromocionDescuentoAsync(long negocioId, long productoId, long stockId, decimal detallePrecio)
        {
            var stock = await ObtenerStockAsync(negocioId, productoId, stockId);
            var promocion = stock.Producto?.Promocion;

            if (promocion == null || promocion.TipoPromocion != TipoPromocion.DESCUENTO)
            {
                throw new ApiException($"El producto {stock.Producto.Nombre} no tiene una promoción de descuento activa");
            }

            decimal precioPromocion = stock.PrecioVenta - (stock.PrecioVenta * (decimal)promocion.Descuento / 100);

            if (detallePrecio != precioPromocion)
            {
                throw new ApiException($"El precio del producto {stock.Producto.Nombre} no coincide con el precio receptado desde backend");
            }
        }

        public async Task VerificarPrecioConPromocionRegaloAsync(long negocioId, long productoId, long stockId)
        {
            var stock = await ObtenerStockAsync(negocioId, productoId, stockId);
            var promocion = stock.Producto?.Promocion;

            if (promocion == null || promocion.TipoPromocion != TipoPromocion.REGALO)
            {
                throw new ApiException($"El producto {stock.Producto.Nombre} no tiene una promoción de regalo activa");
            }
        }

        public async Task VerificarCasosPromocionAsync(DetalleDTO detalle, Promocion? promocion, Venta venta)
        {
             switch (promocion.TipoPromocion)
             {
                 case TipoPromocion.DESCUENTO:
                    await VerificarPrecioConPromocionDescuentoAsync(venta.NegocioId, detalle.ProductoId, detalle.StockId, detalle.Precio);
                 break;

                 case TipoPromocion.REGALO:
                    await VerificarPrecioConPromocionRegaloAsync(venta.NegocioId, detalle.ProductoId, detalle.StockId);
                  break;

                 default:
                    throw new ApiException($"Tipo de promoción no válido para el producto {detalle.ProductoId}");
                }
            
        }

        public async Task<Stock> ObtenerStockAsync(long negocioId, long productoId, long stockId)
        {
            var stockFound = await _stockRepository.FirstOrDefaultAsync(new StockSpecification(negocioId, productoId, stockId))
                ?? throw new ApiException($"No se encontró el stock del producto {productoId} en el proceso de obtención de su stock");

            if(stockFound.Cantidad == 0) throw new ApiException($"El stock del producto {stockFound.Producto.Nombre} está agotado");

            return stockFound;
        }

        public async Task VerificarStockAsync(List<DetalleDTO> detalles, long negocioId)
        {
            List<string> errores = new List<string>(); // Lista para almacenar los errores

            foreach (var detalle in detalles) 
            {
                var stock = await ObtenerStockAsync(negocioId, detalle.ProductoId, detalle.StockId);

                if (stock == null)
                {
                    errores.Add($"No se encontró stock para el producto con ID {detalle.ProductoId} y stock ID {detalle.StockId}");
                    continue; 
                }

                if (stock.Cantidad < detalle.Cantidad)
                {
                    errores.Add($"No hay suficiente stock del producto '{stock.Producto.Nombre}'. Disponible: {stock.Cantidad}, Requerido: {detalle.Cantidad}");
                }
            }

            if (errores.Any())
            {
                throw new ApiException("Se encontraron problemas con el stock:\n" + string.Join("\n", errores) + "\n");
            }


        }



    }
}
