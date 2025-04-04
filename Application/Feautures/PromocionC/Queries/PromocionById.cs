using Application.DTOs.Productos;
using Application.DTOs.Promociones;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.PermissionS;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.PromocionC.Queries
{
    public class PromocionById : IRequest<Response<PromocionInfo>>
    {
        public long Id { get; set; }
        public required long NegocioId { get; set; }

        public required string UserId { get; set; }

        public class PromocionByIdHandler : IRequestHandler<PromocionById, Response<PromocionInfo>>
        {

            private readonly IReadOnlyRepositoryAsync<Promocion> _repository;
            private readonly IPermissionService _permissionService;

            public PromocionByIdHandler(IReadOnlyRepositoryAsync<Promocion> repository, IPermissionService permissionService)
            {
                _repository = repository;
                _permissionService = permissionService;
            }

            public async Task<Response<PromocionInfo>> Handle(PromocionById request, CancellationToken cancellationToken)
            {
                _permissionService.ValidateNegocioPermission(request.NegocioId, request.UserId).Wait(cancellationToken);

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

    public class PromocionByIdParameters
    {
        public int Id { get; set; }
        public long NegocioId { get; set; }
    }
}
