using Application.DTOs.Detalles;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.StockS;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Promocion;
using MediatR;

namespace Application.Feautures.VentaC.Commands
{
    public class CrearVenta : IRequest<Response<long>>
    {
        public DateTime Fecha { get; set; }

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
            private readonly StockService _stockService;
            private readonly IUnitOfWork _unitOfWork;

            public CrearVentaHandler(
                IRepositoryAsync<Venta> repositoryVenta,
                IRepositoryAsync<Detalle> repositoryDetalle,
                IReadOnlyRepositoryAsync<Producto> productoRepostoryReading,
                IReadOnlyRepositoryAsync<Promocion> promocionRepostoryReading,
                StockService stockService,
                IUnitOfWork unitOfWork
                )
            {
                _repositoryVenta = repositoryVenta;
                _repositoryDetalle = repositoryDetalle;
                _productoRepostoryReading = productoRepostoryReading;
                _promocionRepostoryReading = promocionRepostoryReading;
                _stockService = stockService;
                _unitOfWork = unitOfWork;
            }

            public async Task<Response<long>> Handle(CrearVenta request, CancellationToken cancellationToken)
            {
                await _unitOfWork.BeginTransactionAsync(); 

                try
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

                    await _stockService.VerificarStockAsync(request.Detalles, request.NegocioId);

                    foreach (var detalle in request.Detalles)
                    {     
                        Promocion? promocion = null;
                        Detalle detalleToSve = null;
                        var cantidadBeforeRegalo = detalle.Cantidad;

                        if (detalle.PromocionId != 0)
                        {
                            promocion = await _promocionRepostoryReading.FirstOrDefaultAsync
                                (new PromocionSpecification(detalle.ProductoId, detalle.PromocionId), cancellationToken) ?? throw new ApiException("Promocion no encontrada para el producto con Id: "+detalle.ProductoId);

                            await _stockService.VerificarCasosPromocionAsync(detalle, promocion, venta);

                        }
                        else await _stockService.VerificarPrecioAsync(detalle.ProductoId, detalle.StockId, detalle.Precio, detalle.Cantidad, detalle.Total);

                        var precioConIva = await _stockService.AplicarIva(detalle.ProductoId, request.NegocioId, detalle.Precio);

                        decimal precioFinal = precioConIva;
                        int cantidadFinal = detalle.Cantidad;

                        if(promocion != null && promocion.TipoPromocion == TipoPromocion.REGALO)
                        {
                            detalleToSve = new Detalle
                            {
                                Precio = precioFinal,
                                Cantidad = cantidadFinal,
                                Total = detalle.Total,
                                TotalConIva = precioConIva * cantidadBeforeRegalo,
                                ProductoId = detalle.ProductoId,
                                VentaId = venta.Id,
                                StockId = detalle.StockId,
                                PromocionId = promocion == null ? null : promocion.Id
                            };
                        }else if(promocion != null && promocion.TipoPromocion == TipoPromocion.DESCUENTO)
                        {
                            detalleToSve = new Detalle
                            {
                                Precio = precioFinal,
                                Cantidad = detalle.Cantidad,
                                Total = detalle.Total,
                                TotalConIva = precioConIva * detalle.Cantidad,
                                ProductoId = detalle.ProductoId,
                                VentaId = venta.Id,
                                StockId = detalle.StockId,
                                PromocionId = promocion == null ? null : promocion.Id
                            };
                        }
                        else
                        {
                            detalleToSve = new Detalle
                            {
                                Precio = detalle.Precio,
                                Cantidad = cantidadFinal,
                                Total = detalle.Total,
                                TotalConIva = precioConIva * cantidadFinal,
                                ProductoId = detalle.ProductoId,
                                VentaId = venta.Id,
                                StockId = detalle.StockId,
                                PromocionId = promocion == null ? null : promocion.Id
                            };

                        }
                       
                        await _repositoryDetalle.AddAsync(detalleToSve);
                        await _repositoryDetalle.SaveChangesAsync();

                        subtotalCalculado += detalleToSve.Total;
                        totalCalculado += (decimal)detalleToSve.TotalConIva;

                        await _stockService.RestarStock(
                            detalleToSve.Cantidad,
                            detalleToSve.ProductoId,
                            detalle.StockId);
                    }

                    venta.Subtotal = subtotalCalculado;
                    venta.Total = totalCalculado;
                    await _repositoryVenta.UpdateAsync(venta);
                    await _repositoryVenta.SaveChangesAsync();

                    await _unitOfWork.CommitAsync(); // Confirmar transacción

                    return new Response<long>(venta.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR en CrearVentaHandler: {ex.Message}");
                    await _unitOfWork.RollbackAsync(); // ✅ Revertir en caso de error
                    throw;
                }
            }


        }
    }
}
