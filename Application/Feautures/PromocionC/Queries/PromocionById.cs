using Application.DTOs.Productos;
using Application.DTOs.Promociones;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.PromocionC.Queries
{
    public class PromocionById : IRequest<Response<PromocionInfo>>
    {
        public long Id { get; set; }

        public class PromocionByIdHandler : IRequestHandler<PromocionById, Response<PromocionInfo>>
        {

            private readonly IReadOnlyRepositoryAsync<Promocion> _repository;

            public PromocionByIdHandler(IReadOnlyRepositoryAsync<Promocion> repository)
            {
                _repository = repository;
            }

            public async Task<Response<PromocionInfo>> Handle(PromocionById request, CancellationToken cancellationToken)
            {
                var promocionFound = await _repository.FirstOrDefaultAsync(new PromocionSpecification(request.Id));
                if (promocionFound == null) throw new ApiException($"Promocion con Id {request.Id} no encontrada");

                var promocionMapped = new PromocionInfo{
                    Id = promocionFound.Id,
                    TipoPromocion = promocionFound.TipoPromocion,
                    Descuento = promocionFound.Descuento,
                    CantidadCompra = promocionFound.CantidadCompra,
                    CantidadGratis = promocionFound.CantidadGratis,
                    FechaInicio = promocionFound.FechaInicio,
                    FechaFin = promocionFound.FechaFin,
                    Productos = promocionFound.Productos.Select(p => new ProductoDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        CategoriaId = p.CategoriaId,
                        Codigo = p.Codigo,
                        Estado = p.Estado,
                        Iva = p.Iva,
                    }).ToList()
                };
                return new Response<PromocionInfo>(promocionMapped);

            }
        }
    }
}
