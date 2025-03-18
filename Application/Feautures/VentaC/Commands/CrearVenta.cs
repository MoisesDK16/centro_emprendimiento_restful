using Application.DTOs.Detalles;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.StockS;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Promocion;
using MediatR;
using System.Composition;

namespace Application.Feautures.VentaC.Commands
{
    public class CrearVenta : IRequest<Response<long>>
    {
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        //Relaciones
        public int ClienteId { get; set; }
        public int NegocioId { get; set; }
        public required List<DetalleDTO> Detalles { get; set; }

        public class CrearVentaHandler : IRequestHandler<CrearVenta, Response<long>>
        {
            private readonly IRepositoryAsync<Venta> _repositoryVenta;
            private readonly IRepositoryAsync<Detalle> _repositoryDetalle;
            private readonly IReadOnlyRepositoryAsync<Producto> _productoRepostoryReading;
            private readonly IReadOnlyRepositoryAsync<Promocion> _promocionRepostoryReading;
            private readonly StockService _stockService;  // <-------- como hago aqui??

            public CrearVentaHandler(
                IRepositoryAsync<Venta> repositoryVenta,
                IRepositoryAsync<Detalle> repositoryDetalle,
                IReadOnlyRepositoryAsync<Producto> productoRepostoryReading,
                IReadOnlyRepositoryAsync<Promocion> promocionRepostoryReading,
                StockService stockService)
            {
                _repositoryVenta = repositoryVenta;
                _repositoryDetalle = repositoryDetalle;
                _productoRepostoryReading = productoRepostoryReading;
                _promocionRepostoryReading = promocionRepostoryReading;
                _stockService = stockService;
            }

            public async Task<Response<long>> Handle(CrearVenta request, CancellationToken cancellationToken)
            {
                foreach (var detalle in request.Detalles)
                {
                    var producto = await _productoRepostoryReading.GetByIdAsync(detalle.ProductoId);
                    if (producto == null) throw new ApiException($"Producto con ID {detalle.ProductoId} no encontrado.");
                }

                var venta = new Venta
                {
                    Fecha = request.Fecha,
                    Subtotal = 0,  
                    Total = 0,     
                    ClienteId = request.ClienteId,
                    NegocioId = request.NegocioId,
                    Detalles = new List<Detalle>() 
                };

                venta = await _repositoryVenta.AddAsync(venta);
                await _repositoryVenta.SaveChangesAsync();

                decimal subtotalCalculado = 0;
                decimal totalCalculado = 0;

                foreach (var detalle in request.Detalles)
                {
                    var promocion = detalle.PromocionId != 0 ?
                        await _promocionRepostoryReading.FirstOrDefaultAsync(new PromocionSpecification(detalle.ProductoId), cancellationToken) : null;

                    Console.WriteLine("Promoción de detalle: " + (promocion != null ? promocion.TipoPromocion.ToString() : "Ninguna"));

                    decimal precioFinal = detalle.Precio;
                    int cantidadFinal = detalle.Cantidad;

                    if (promocion != null)
                    {
                        if (promocion.TipoPromocion == TipoPromocion.DESCUENTO)
                        {
                            precioFinal = detalle.Precio - (detalle.Precio * (promocion.Descuento / 100));
                        }
                        if (promocion.TipoPromocion == TipoPromocion.DOS_POR_UNO)
                        {
                            cantidadFinal += promocion.CantidadGratis;
                        }
                    }

                    var nuevoDetalle = new Detalle
                    {
                        Precio = precioFinal,
                        Cantidad = cantidadFinal,
                        Total = precioFinal * cantidadFinal,
                        ProductoId = detalle.ProductoId,
                        VentaId = venta.Id,
                        PromocionId = promocion == null ? null : promocion.Id
                    };

                    subtotalCalculado += nuevoDetalle.Total;
                    totalCalculado += nuevoDetalle.Total;

                    await _repositoryDetalle.AddAsync(nuevoDetalle);

                    _stockService.RestarStock(
                        nuevoDetalle.Cantidad,
                        nuevoDetalle.ProductoId,
                        detalle.StockId);
                }

                venta.Subtotal = subtotalCalculado;
                venta.Total = totalCalculado;
                await _repositoryVenta.UpdateAsync(venta);
                await _repositoryVenta.SaveChangesAsync();

                return new Response<long>(venta.Id);
            }

        }
    }
}
