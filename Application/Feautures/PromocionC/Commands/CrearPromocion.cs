using Application.Exceptions;
using Application.Feautures.PromocionC.Queries;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Promocion;
using MediatR;

namespace Application.Feautures.PromocionC.Commands
{
    public class CrearPromocion : IRequest<Response<long>>
    {
        public TipoPromocion TipoPromocion { get; set; }
        public decimal? Descuento { get; set; }
        public int? CantidadCompra { get; set; }
        public int? CantidadGratis { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public long NegocioId { get; set; }
        public List<long> IdProductos { get; set; } = new List<long>();


        public class CrearPromocionHandler : IRequestHandler<CrearPromocion, Response<long>>
        {
            private readonly IRepositoryAsync<Promocion> _repository;
            private readonly IRepositoryAsync<Producto> _productoRepository;
            private readonly IReadOnlyRepositoryAsync<Producto> _productoyReading;
            private readonly IRepositoryAsync<Domain.Entities.Negocio> _negocioRepository;
            private readonly IReadOnlyRepositoryAsync<Domain.Entities.Negocio> _negocioRepositoryReading;
            public CrearPromocionHandler(
                IRepositoryAsync<Promocion> repository,
                IRepositoryAsync<Producto> productoRepository,
                IReadOnlyRepositoryAsync<Producto> productoyReading,
                IReadOnlyRepositoryAsync<Domain.Entities.Negocio> negocioRepositoryReading)
            {
                _repository = repository;
                _productoRepository = productoRepository;
                _productoyReading = productoyReading;
                _negocioRepositoryReading = negocioRepositoryReading;
            }

            public async Task<Response<long>> Handle(CrearPromocion request, CancellationToken cancellationToken)
            {
                var negocioFound = await _negocioRepositoryReading.GetByIdAsync(request.NegocioId, cancellationToken);
                if (negocioFound == null)
                    throw new ApiException($"Negocio con Id {request.NegocioId} no encontrado");

                var productosFound = (await Task.WhenAll(request.IdProductos.Select(async id =>
                    await _productoyReading.FirstOrDefaultAsync(new ProductoSpecification(id, true))
                ))).Where(p => p != null).ToList();

                if (!productosFound.Any())
                    throw new ApiException("No se encontraron los productos con los IDs proporcionados");

                var idsNoEncontrados = request.IdProductos.Except(productosFound.Select(p => p.Id)).ToList();
                if (idsNoEncontrados.Any())
                    throw new ApiException($"No se encontraron los productos con los IDs: {string.Join(", ", idsNoEncontrados)}");

                if (request.FechaInicio >= request.FechaFin)
                    throw new ApiException("La fecha de inicio no puede ser mayor o igual a la fecha de fin.");

                Promocion promocion = request.TipoPromocion switch
                {
                    TipoPromocion.DESCUENTO => await CrearYGuardarPromocionDescuentoAsync(request, productosFound),
                    TipoPromocion.REGALO => await CrearYGuardarPromocionRegaloAsync(request, productosFound),
                    _ => throw new ApiException("Tipo de promoción no válido.")
                };

                return new Response<long>(promocion.Id);
            }

            private async Task<Promocion> CrearYGuardarPromocionDescuentoAsync(CrearPromocion request, List<Producto> productosFound)
            {
                if (request.Descuento is null || request.Descuento <= 0 || request.Descuento >= 100)
                    throw new ApiException("El descuento debe ser mayor a 0 y menor a 100.");

                var promocion = new Promocion
                {
                    TipoPromocion = TipoPromocion.DESCUENTO,
                    Descuento = request.Descuento.Value,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin,
                    NegocioId = request.NegocioId,
                    Estado = Estado.ACTIVA
                };

                return await GuardarPromocionAsync(promocion, productosFound);
            }

            private async Task<Promocion> CrearYGuardarPromocionRegaloAsync(CrearPromocion request, List<Producto> productosFound)
            {
                if (request.CantidadCompra is null || request.CantidadCompra <= 0)
                    throw new ApiException("La cantidad de compra debe ser mayor a 0.");

                if (request.CantidadGratis is null || request.CantidadGratis <= 0)
                    throw new ApiException("La cantidad gratis debe ser mayor a 0.");

                var promocion = new Promocion
                {
                    TipoPromocion = TipoPromocion.REGALO,
                    CantidadCompra = request.CantidadCompra.Value,
                    CantidadGratis = request.CantidadGratis.Value,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin,
                    NegocioId = request.NegocioId,
                    Estado = Estado.ACTIVA
                };

                return await GuardarPromocionAsync(promocion, productosFound);
            }

            private async Task<Promocion> GuardarPromocionAsync(Promocion promocion, List<Producto> productosFound)
            {
                Console.WriteLine("Guardando promocion");

                if (!Enum.IsDefined(typeof(Estado), promocion.Estado))
                {
                    promocion.Estado = Estado.ACTIVA;
                }

                Console.WriteLine("Promocion: " + promocion);
                await _repository.AddAsync(promocion);
                await _repository.SaveChangesAsync();

                if (promocion.Id <= 0)
                    throw new ApiException("Error al generar el ID de la promoción.");

                Console.WriteLine("Promocion guardada");

                foreach (var producto in productosFound)
                {
                    producto.PromocionId = promocion.Id;
                }

                await _productoRepository.UpdateRangeAsync(productosFound);
                await _productoRepository.SaveChangesAsync();

                return promocion;
            }



        }
    }
}
