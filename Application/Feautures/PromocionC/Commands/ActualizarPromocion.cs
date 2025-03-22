using Application.Exceptions;
using Application.Feautures.PromocionC.Queries;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Uno.Extensions;
using Uno.Extensions.Specialized;

namespace Application.Feautures.PromocionC.Commands
{
    public class ActualizarPromocion : IRequest<Response<long>>
    {
        public long Id { get; set; }
        public decimal Descuento { get; set; }
        public int CantidadCompra { get; set; }
        public int CantidadGratis { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<long> IdProductos { get; set; } = [];


        public class ActualizarPromocionHandler : IRequestHandler<ActualizarPromocion, Response<long>>
        {
            private readonly IRepositoryAsync<Promocion> _repository;
            private readonly IRepositoryAsync<Producto> _productoRepository;
            private readonly IReadOnlyRepositoryAsync<Producto> _productoyReading;
            private readonly IReadOnlyRepositoryAsync<Promocion> _promocionRepositoryReading;
            public ActualizarPromocionHandler(IRepositoryAsync<Promocion> repository, IRepositoryAsync<Producto> productoRepository, IReadOnlyRepositoryAsync<Producto> productoyReading, IReadOnlyRepositoryAsync<Promocion> promocionRepositoryReading)
            {
                _repository = repository;
                _productoRepository = productoRepository;
                _productoyReading = productoyReading;
                _promocionRepositoryReading = promocionRepositoryReading;
            }

            public async Task<Response<long>> Handle(ActualizarPromocion request, CancellationToken cancellationToken)
            {

                var productosFound = new List<Producto>();

                // Obtener los productos con el mismo contexto (_productoRepository)
                foreach (var id in request.IdProductos)
                {
                    var producto = await _productoRepository.GetByIdAsync(id);
                    if (producto != null)
                    {
                        productosFound.Add(producto);
                    }
                }

                // Validar productos no encontrados
                var idsNoEncontrados = request.IdProductos.Except(productosFound.Select(p => p.Id)).ToList();

                if (idsNoEncontrados.Any())
                {
                    throw new ApiException($"No se encontraron los productos con los IDs: {string.Join(", ", idsNoEncontrados)}");
                }

                var promocion = await _promocionRepositoryReading.FirstOrDefaultAsync(new PromocionSpecification(request.Id)) ?? throw new ApiException("No se encontró promoción promocion con id: "+request.Id);

                var productosAntiguos = promocion?.Productos?
                    .Where(p => !productosFound.Any(nuevo => nuevo.Id == p.Id))
                    .ToList();

                if(productosAntiguos != null)
                {
                    foreach (var prod in productosAntiguos)
                    {
                        Console.WriteLine("PRODUCTOS ANTIGUOS" + prod.Id);
                        prod.PromocionId = null;
                        prod.Promocion = null;
                        await _productoRepository.UpdateAsync(prod);
                        promocion.Productos.Remove(prod);
                    }
                }

                foreach (var producto in productosFound)
                {
                    producto.PromocionId = promocion.Id;
                    promocion.Productos.Add(producto);
                }

                var productosAActualizar = (productosFound);
                await _productoRepository.UpdateRangeAsync(productosAActualizar);
                await _productoRepository.SaveChangesAsync();

                promocion.Descuento = request.Descuento;
                promocion.CantidadCompra = request.CantidadCompra;
                promocion.CantidadGratis = request.CantidadGratis;
                promocion.FechaInicio = request.FechaInicio;
                promocion.FechaFin = request.FechaFin;

                await _repository.UpdateAsync(promocion);
                await _repository.SaveChangesAsync();

                return new Response<long>(promocion.Id);
            }


        }
    }
}
