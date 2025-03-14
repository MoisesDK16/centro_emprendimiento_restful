using Application.Exceptions;
using Application.Feautures.PromocionC.Queries;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Promocion;
using MediatR;

namespace Application.Feautures.PromocionC.Commands
{
    public class CrearPromocion : IRequest<Response<long>>
    {
        public TipoPromocion TipoPromocion { get; set; }
        public decimal Descuento { get; set; }
        public int CantidadCompra { get; set; }
        public int CantidadGratis { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<long> IdProductos { get; set; } = new List<long>();


        public class CrearPromocionHandler : IRequestHandler<CrearPromocion, Response<long>>
        {
            private readonly IRepositoryAsync<Promocion> _repository;
            private readonly IRepositoryAsync<Producto> _productoRepository;
            private readonly IReadOnlyRepositoryAsync<Producto> _productoyReading;
            public CrearPromocionHandler(IRepositoryAsync<Promocion> repository, IRepositoryAsync<Producto> productoRepository, IReadOnlyRepositoryAsync<Producto> productoyReading)
            {
                _repository = repository;
                _productoRepository = productoRepository;
                _productoyReading = productoyReading;
            }

            public async Task<Response<long>> Handle(CrearPromocion request, CancellationToken cancellationToken)
            {
                var productosFound = await _productoyReading.ListAsync(new ProductoSpecification(request.IdProductos));

                if (productosFound.Count != request.IdProductos.Count)
                {
                    var idsNoEncontrados = request.IdProductos.Except(productosFound.Select(p => p.Id));
                    throw new ApiException($"No se encontraron los productos con los IDs: {string.Join(", ", idsNoEncontrados)}");
                }

                var promocion = new Promocion
                {
                    TipoPromocion = request.TipoPromocion,
                    Descuento = request.Descuento,
                    CantidadCompra = request.CantidadCompra,
                    CantidadGratis = request.CantidadGratis,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin
                };

                await _repository.AddAsync(promocion);
                await _repository.SaveChangesAsync(); 

                foreach (var producto in productosFound)
                {
                    producto.PromocionId = promocion.Id; 
                }

                await _productoRepository.UpdateRangeAsync(productosFound);
                await _productoRepository.SaveChangesAsync(); 

                return new Response<long>(promocion.Id);
            }


        }
    }
}
