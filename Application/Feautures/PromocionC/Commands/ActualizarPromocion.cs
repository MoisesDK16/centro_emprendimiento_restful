using Application.Exceptions;
using Application.Feautures.PromocionC.Queries;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public ActualizarPromocionHandler(IRepositoryAsync<Promocion> repository, IRepositoryAsync<Producto> productoRepository, IReadOnlyRepositoryAsync<Producto> productoyReading)
            {
                _repository = repository;
                _productoRepository = productoRepository;
                _productoyReading = productoyReading;
            }
            public async Task<Response<long>> Handle(ActualizarPromocion request, CancellationToken cancellationToken)
            {
                var productosFound = await _productoyReading.ListAsync(new ProductoSpecification(request.IdProductos));
                if (productosFound.Count != request.IdProductos.Count)
                {
                    var idsNoEncontrados = request.IdProductos.Except(productosFound.Select(p => p.Id));
                    throw new ApiException($"No se encontraron los productos con los IDs: {string.Join(", ", idsNoEncontrados)}");
                }
                var promocion = await _repository.GetByIdAsync(request.Id);

                promocion.Descuento = request.Descuento;
                promocion.CantidadCompra = request.CantidadCompra;
                promocion.CantidadGratis = request.CantidadGratis;
                promocion.FechaInicio = request.FechaInicio;
                promocion.FechaFin = request.FechaFin;
                productosFound.ForEach(p => promocion.Productos.Add(p));

                await _repository.UpdateAsync(promocion);
                await _repository.SaveChangesAsync();

                productosFound.ForEach(p => p.PromocionId = promocion.Id);
                await _productoRepository.UpdateRangeAsync(productosFound);
                await _productoRepository.SaveChangesAsync();
                return new Response<long>(promocion.Id);
            }
        }
    }
}
